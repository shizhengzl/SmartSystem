using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Core.UsuallyCommon
{
    /// <summary>
    /// 文件扩展
    /// </summary>
    public static class FileExtenstion
    {
        /// <summary>
        /// 根据路劲获取文件内容
        /// </summary>
        /// <param name="Paht">路劲</param>
        /// <returns></returns>
        public static string GetFileContext(this string Paht)
        {
            return File.ReadAllText(Paht, Encoding.Default);
        }

        /// <summary>  
        /// 向文本文件中写入内容  
        /// </summary>  
        /// <param name="filePath">文件的绝对路径</param>  
        /// <param name="content">写入的内容</param>          
        public static void WriteText(this string filePath, string content)
        {
            //向文件写入内容  
            File.WriteAllText(filePath, content, Encoding.Default);
        }



        /// <summary>
        /// 获取文件夹下所有扩展文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ext"></param>
        /// <param name="list"></param>
        public static void GetFileByExtension(string filePath, string ext, ref List<string> list)
        {

            if (!System.IO.Directory.Exists(filePath))
                return;
            try
            {
                DirectoryInfo folder = new DirectoryInfo(filePath);
                FileInfo[] chldFiles = folder.GetFiles(ext);
                foreach (FileInfo chlFile in chldFiles)
                {
                    list.Add(chlFile.FullName);
                }
                DirectoryInfo[] chldFolders = folder.GetDirectories();
                foreach (DirectoryInfo chldFolder in chldFolders)
                {
                    GetFileByExtension(chldFolder.FullName, ext, ref list);
                }
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}


