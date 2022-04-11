using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MSApp1.ShellFileOperation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MSApp1
{
    public static class FileOperation
    {
        
        public enum FileOperations
        {
            FO_MOVE = 0x0001,		// Move the files specified in pFrom to the location specified in pTo. 
            FO_COPY = 0x0002,		// Copy the files specified in the pFrom member to the location specified 
            // in the pTo member. 
            FO_DELETE = 0x0003,		// Delete the files specified in pFrom. 
            FO_RENAME = 0x0004		// Rename the file specified in pFrom. You cannot use this flag to rename 
            // multiple files with a single function call. Use FO_MOVE instead. 
        }

        public static FileOperations Operation;
        public static IntPtr OwnerWindow;
        public static ShellFileOperationFlags OperationFlags;
        public static String ProgressTitle;

        public static ShellNameMapping[] NameMappings;

        private static bool MakeOperationsWithFiles(uint Operation,string Source, string Dest)
        {
            ShellApi.SHFILEOPSTRUCT FileOpStruct = new ShellApi.SHFILEOPSTRUCT();

            FileOpStruct.hwnd = OwnerWindow;
            FileOpStruct.wFunc = (uint)Operation;

            FileOpStruct.pFrom = Marshal.StringToHGlobalUni(Source);
            FileOpStruct.pTo = Marshal.StringToHGlobalUni(Dest);

            FileOpStruct.fFlags = (ushort)OperationFlags;
            FileOpStruct.lpszProgressTitle = ProgressTitle;
            FileOpStruct.fAnyOperationsAborted = 0;
            FileOpStruct.hNameMappings = IntPtr.Zero;

            int RetVal;
            RetVal = ShellApi.SHFileOperation(ref FileOpStruct);

            ShellApi.SHChangeNotify(
                (uint)ShellChangeNotificationEvents.SHCNE_ALLEVENTS,
                (uint)ShellChangeNotificationFlags.SHCNF_DWORD,
                IntPtr.Zero,
                IntPtr.Zero);

            if (RetVal != 0)
                return false;

            if (FileOpStruct.fAnyOperationsAborted != 0)
                return false;

            return true;
        }

        public static bool CopyFilesWithMasks(string srcFolder, string dstFolder, string mask)
        {
            return MakeOperationsWithFiles(0x0002, MasksToMultiMask(srcFolder, mask), dstFolder);
        }
        public static bool MoveFilesWithMasks(string srcFolder, string dstFolder, string mask)
        {
            return MakeOperationsWithFiles(0x0001, MasksToMultiMask(srcFolder, mask), dstFolder);
        }
        public static bool DeleteFilesWithMasks(string srcFolder,  string mask)
        {
            return MakeOperationsWithFiles(0x0003, MasksToMultiMask(srcFolder, mask),null);
        }

        /// /with List

        public static bool CopyFilesByList(ObservableCollection<string> files, string destFolder)
        {
            return MakeOperationsWithFiles(0x0002, StringsToMultiString(files), destFolder);
        }
        public static bool MoveFilesByList(ObservableCollection<string> files, string destFolder)
        {
            return MakeOperationsWithFiles(0x0001, StringsToMultiString(files), destFolder);
        }
        public static bool DeleteFilesByList(ObservableCollection<string> files)
        {
            return MakeOperationsWithFiles(0x0003, StringsToMultiString(files),null);
        }

        private static string StringsToMultiString(ObservableCollection<string> strings)
        {
            string str = "";

            // if null or if count == 0 then return empty string
            if ((strings?.Count ?? 0) == 0)
                return str;

            for (int i = 0; i < strings.Count; ++i)
                str += strings[i] + '\0';
            str += '\0';

            return str;
        }

        private static string MasksToMultiMask(string sourceFolder, string mask)
        {
            ObservableCollection<string> temp = new ObservableCollection<string>();
            string[] masks;
            char[] separator = new char[] { '|' };

            masks = mask.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < masks.Length; ++i)
            {
                string[] filesWithMask = Directory.GetFiles(sourceFolder, masks[i]);
                int filesWithMaskCount = filesWithMask.Length;

                for (int j = 0; j < filesWithMaskCount; ++j)
                    if (!(temp.Contains(filesWithMask[j])))
                        temp.Add(filesWithMask[j]);
            }
            return StringsToMultiString(temp);
        }

        public static void RenameFile(string Source, string newName)
        {
            string dest = Source.Substring(0, Source.LastIndexOf('\\') + 1) + newName;
            Source += '\0';
            MakeOperationsWithFiles(0x0004, Source, dest);
        }
    }
}
