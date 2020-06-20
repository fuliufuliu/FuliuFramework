﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

///<summary>
///</summary>
public static class FileHelper {

    /// <summary>
    /// 计算文件的MD5并返回
    /// </summary>
    public static string GetMd5Hash(string filePath)
    {
        MD5 md5Hash = MD5.Create();
        Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] data = md5Hash.ComputeHash(stream);
        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        stream.Close();
        return sBuilder.ToString();
    }

    public static void foreachDirectory(string directoryPath, Action<FileSystemInfo> eachCall)
    {
        if (IsExistDirectory(directoryPath))
        {
            var dirInfo = new DirectoryInfo(directoryPath);
            foreachDirectory(dirInfo, eachCall);
        }
    }

    public static void foreachDirectory(DirectoryInfo dirInfo, Action<FileSystemInfo> eachCall)
    {
        if (eachCall != null && dirInfo != null)
        {
            FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] is DirectoryInfo)
                {
                    foreachDirectory(files[i] as DirectoryInfo, eachCall);
                }
                else
                {
                    eachCall(files[i]);
                }
            }
        }
    }

    /// <summary>
    /// 获取目录（或文件路径）中上一级目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string getParentPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        path.Replace("\\", "/");

        recheck:
        if (path.EndsWith("/"))
        {
            path = path.Substring(0, path.Length - 1);
            goto recheck;
        }

        if (path.Contains("/"))
        {
            return path.Substring(0, path.LastIndexOf("/"));
        }
        return null;
    }

    #region 检测指定目录是否存在

    /// <summary>
    /// 检测指定目录是否存在
    /// </summary>
    /// <param name="directoryPath">目录的绝对路径</param>        
    public static bool IsExistDirectory(string directoryPath) {
        return Directory.Exists(directoryPath);
    }

    #endregion

    #region 检测指定文件是否存在

    /// <summary>
    /// 检测指定文件是否存在,如果存在则返回true。
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static bool IsExistFile(string filePath) {
        return File.Exists(filePath);
    }

    #endregion

    #region 检测指定目录是否为空

    /// <summary>
    /// 检测指定目录是否为空
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>        
    public static bool IsEmptyDirectory(string directoryPath) {
        try {
            //判断是否存在文件
            string[] fileNames = GetFileNames(directoryPath);
            if (fileNames.Length > 0) {
                return false;
            }

            //判断是否存在文件夹
            string[] directoryNames = GetDirectories(directoryPath);
            return directoryNames.Length <= 0;
        }
        catch {
            return false;
        }
    }

    #endregion

    #region 检测指定目录中是否存在指定的文件

    /// <summary>
    /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
    /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
    public static bool Contains(string directoryPath, string searchPattern) {
        try {
            //获取指定的文件列表
            string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

            //判断指定文件是否存在
            return fileNames.Length != 0;
        }
        catch {
            return false;
        }
    }

    /// <summary>
    /// 检测指定目录中是否存在指定的文件
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
    /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
    /// <param name="isSearchChild">是否搜索子目录</param>
    public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild) {
        try {
            //获取指定的文件列表
            string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

            //判断指定文件是否存在
            return fileNames.Length != 0;
        }
        catch {
            return false;
        }
    }

    #endregion

    #region 创建一个目录

    /// <summary>
    /// 创建一个目录
    /// </summary>
    /// <param name="directoryPath">目录的绝对路径</param>
    public static void CreateDirectory(string directoryPath) {
        //如果目录不存在则创建该目录
        if (!IsExistDirectory(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
            return;
        }
        Debug.LogErrorFormat("创建 {0} 目录失败！目录已存在！", directoryPath);
    }

    #endregion

    public static void CreateDirectoryForFileSave(string filePath)
    {
        string directoryPath = GetDirectoryPathFromFilePath(filePath);
		if(!IsExistDirectory(directoryPath))
        	CreateDirectory(directoryPath);
    }

    public static string GetDirectoryPathFromFilePath(string filePath)
    {
        if (filePath != null)
        {
            filePath = filePath.Replace("\\\\", "/").Replace("\\", "/");
            if (filePath.Contains("/"))
            {
                return filePath.Substring(0, filePath.LastIndexOf("/"));
            }
        }
        return filePath;
    }

    #region 创建一个文件

    /// <summary>
    /// 创建一个文件。
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    public static bool CreateFile(string filePath) {
        try {
            //如果文件不存在则创建该文件
            if (!IsExistFile(filePath))
            {
                string directoryPath = GetDirectoryPathFromFilePath(filePath);
                CreateDirectory(directoryPath);
                //创建一个FileInfo对象
                FileInfo file = new FileInfo(filePath);
                //创建文件
                FileStream fs = file.Create();
                //关闭文件流
                fs.Close();
            }
        }
        catch {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 创建一个文件,并将字节流写入文件。
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    /// <param name="buffer">二进制流数据</param>
    public static bool CreateFile(string filePath, byte[] buffer) {
        try {
            //如果文件不存在则创建该文件
            if (!IsExistFile(filePath)) {
                //创建一个FileInfo对象
                FileInfo file = new FileInfo(filePath);

                //创建文件
                FileStream fs = file.Create();

                //写入二进制流
                fs.Write(buffer, 0, buffer.Length);

                //关闭文件流
                fs.Close();
            }
        }
        catch {
            return false;
        }
        return true;
    }

    #endregion

    #region 获取文本文件的行数

    /// <summary>
    /// 获取文本文件的行数
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static int GetLineCount(string filePath) {
        //将文本文件的各行读到一个字符串数组中
        string[] rows = File.ReadAllLines(filePath);

        //返回行数
        return rows.Length;
    }

    #endregion

    #region 获取一个文件的长度

    /// <summary>
    /// 获取一个文件的长度,单位为Byte
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static int GetFileSize(string filePath) {
        //创建一个文件对象
        FileInfo fi = new FileInfo(filePath);

        //获取文件的大小
        return (int) fi.Length;
    }

    /// <summary>
    /// 获取一个文件的长度,单位为KB
    /// </summary>
    /// <param name="filePath">文件的路径</param>        
    public static double GetFileSizeByKB(string filePath) {
        //创建一个文件对象
        FileInfo fi = new FileInfo(filePath);
        long size = fi.Length/1024;
        //获取文件的大小
        return double.Parse(size.ToString());
    }

    /// <summary>
    /// 获取一个文件的长度,单位为MB
    /// </summary>
    /// <param name="filePath">文件的路径</param>        
    public static double GetFileSizeByMB(string filePath) {
        //创建一个文件对象
        FileInfo fi = new FileInfo(filePath);
        long size = fi.Length/1024/1024;
        //获取文件的大小
        return double.Parse(size.ToString());
    }

    #endregion

    #region 获取指定目录中的文件列表

    /// <summary>
    /// 获取指定目录中所有文件列表
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>        
    public static string[] GetFileNames(string directoryPath) {
        //如果目录不存在，则抛出异常
        if (!IsExistDirectory(directoryPath)) {
            throw new FileNotFoundException();
        }

        //获取文件列表
        return Directory.GetFiles(directoryPath);
    }

    /// <summary>
    /// 获取指定目录及子目录中所有文件列表
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
    /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
    /// <param name="isSearchChild">是否搜索子目录</param>
    public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild) {
        //如果目录不存在，则抛出异常
        if (!IsExistDirectory(directoryPath)) {
            throw new FileNotFoundException();
        }

        try {
            return Directory.GetFiles(directoryPath, searchPattern,
                isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }
        catch {
            return null;
        }
    }

    #endregion

    #region 获取指定目录中的子目录列表

    /// <summary>
    /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>        
    public static string[] GetDirectories(string directoryPath) {
        try {
            return Directory.GetDirectories(directoryPath);
        }
        catch {
            return null;
        }
    }

    /// <summary>
    /// 获取指定目录及子目录中所有子目录列表
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
    /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
    /// <param name="isSearchChild">是否搜索子目录</param>
    public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild) {
        try {
            return Directory.GetDirectories(directoryPath, searchPattern,
                isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }
        catch {
            throw null;
        }
    }

    #endregion

    #region 向文本文件写入内容

    /// <summary>
    /// 向文本文件中写入内容
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    /// <param name="content">写入的内容</param>        
    public static void WriteText(string filePath, string content)
    {
        CreateDirectoryForFileSave(filePath);
        //向文件写入内容
        File.WriteAllText(filePath, content);
    }

    /// <summary>
    /// 向文件中写入字节内容
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    /// <param name="content">写入的内容</param>     
    public static void WriteBytes(string filePath, byte[] content)
    {
        CreateDirectoryForFileSave(filePath);
        //向文件写入内容
        File.WriteAllBytes(filePath, content);
    }

    #endregion

    #region 向文本文件的尾部追加内容

    /// <summary>
    /// 向文本文件的尾部追加内容
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    /// <param name="content">写入的内容</param>
    public static void AppendText(string filePath, string content) {
        File.AppendAllText(filePath, content);
    }

    #endregion

    #region 将现有文件的内容复制到新文件中

    /// <summary>
    /// 将源文件的内容复制到目标文件中
    /// </summary>
    /// <param name="sourceFilePath">源文件的绝对路径</param>
    /// <param name="destFilePath">目标文件的绝对路径</param>
    public static void Copy(string sourceFilePath, string destFilePath)
    {
        if (IsExistFile(sourceFilePath))
        {
            var destDic = getParentPath(destFilePath);
            if (!IsExistDirectory(destDic))
            {
                CreateDirectory(destDic);
            }
            File.Copy(sourceFilePath, destFilePath, true);
            return;
        }
        throw new Exception(string.Format("{0} 文件不存在！", sourceFilePath));
    }


    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="srcPath">原文件夹</param>
    /// <param name="destPath">目标文件夹</param>
    /// <param name="getNewFileNameFunc">文件名替换方法</param>
    public static void CopyDirectory(string srcPath, string destPath, Func<FileSystemInfo, string> getNewFileNameFunc = null)
    {
        if (IsExistDirectory(srcPath))
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)     //判断是否文件夹
                {
                    if (!Directory.Exists(destPath + "/" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "/" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }

                    CopyDirectory(i.FullName, destPath + "/" + i.Name, getNewFileNameFunc);    //递归调用复制子文件夹
                }
                else
                {
                    var newFileName = i.Name;
                    if (getNewFileNameFunc != null)
                    {
                        newFileName = getNewFileNameFunc(i);
                    }
                    Copy(i.FullName, destPath + "/" + newFileName);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
            return;
        }
        throw new Exception(string.Format("{0} 文件夹不存在！", srcPath));
    }

    #endregion

    #region 将文件移动到指定目录

    /// <summary>
    /// 将文件移动到指定目录
    /// </summary>
    /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
    /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
    public static void Move(string sourceFilePath, string descDirectoryPath) {
        //获取源文件的名称
        string sourceFileName = GetFileName(sourceFilePath);

        if (IsExistDirectory(descDirectoryPath)) {
            //如果目标中存在同名文件,则删除
            if (IsExistFile(descDirectoryPath + "/" + sourceFileName)) { 
                DeleteFile(descDirectoryPath + "/" + sourceFileName);
            }
            //将文件移动到指定目录
            File.Move(sourceFilePath, descDirectoryPath + "/" + sourceFileName);
        }
    }


    /// <summary>
    /// 移动文件夹到一个新位置
    /// </summary>
    /// <param name="srcPath">原文件夹</param>
    /// <param name="destPath">目标文件夹</param>
    /// <param name="getNewFileNameFunc">文件名替换方法</param>
    public static void MoveDirectory(string srcPath, string destPath, Func<FileSystemInfo, string> getNewFileNameFunc = null)
    {
        CopyDirectory(srcPath, destPath, getNewFileNameFunc);
        DeleteDirectory(srcPath);
    }

    #endregion
    
    #region 将文件或文件夹重命名
    /// <summary>
    /// 将文件或文件夹重命名
    /// </summary>
    /// <param name="path">文件或文件夹所在完整路径</param>
    /// <param name="newName">新文件或文件夹名字</param>
    /// <returns></returns>
    public static bool Rename(string path, string newName)
    {
        try
        {
            if (IsExistDirectory(path))
            {
                var parentPath = FileHelper.getParentPath(path);
                Debug.LogFormat(" 文件或文件夹所在完整路径  {0}", parentPath + "/" + newName);
                Directory.Move(path, parentPath + "/" + newName);
                return true;
            }
            if (IsExistFile(path))
            {
                var parentPath = FileHelper.getParentPath(path);
                Debug.LogFormat(" 文件或文件夹所在完整路径  {0}", parentPath + "/" + newName);
                File.Move(path, parentPath + "/" + newName);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogFormat(" 文件或文件夹{0} 重命名失败！error ：{1}", path, ex);
        }
        return false;
    }
    #endregion

    #region 将流读取到缓冲区中

    /// <summary>
    /// 将流读取到缓冲区中
    /// </summary>
    /// <param name="stream">原始流</param>
    public static byte[] StreamToBytes(Stream stream) {
        try {
            //创建缓冲区
            byte[] buffer = new byte[stream.Length];

            //读取流
            stream.Read(buffer, 0, int.Parse(stream.Length.ToString()));

            //返回流
            return buffer;
        }
        catch {
            return null;
        }
        finally {
            //关闭流
            stream.Close();
        }
    }

    #endregion

    #region 将文件读取到缓冲区中

    /// <summary>
    /// 将文件读取到缓冲区中
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    public static byte[] FileToBytes(string filePath) {
        if (!IsExistFile(filePath))
        {
            return null;
        }
        //获取文件的大小 
        int fileSize = GetFileSize(filePath);

        //创建一个临时缓冲区
        byte[] buffer = new byte[fileSize];

        //创建一个文件流
        FileInfo fi = new FileInfo(filePath);
        FileStream fs = fi.Open(FileMode.Open);

        try {
            //将文件流读入缓冲区
            fs.Read(buffer, 0, fileSize);

            return buffer;
        }
        catch {
            return null;
        }
        finally {
            //关闭文件流
            fs.Close();
        }
    }

    #endregion

    #region 将文件读取到字符串中

    /// <summary>
    /// 将文件读取到字符串中
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    public static string FileToString(string filePath) {
        return FileToString(filePath, Encoding.UTF8);
    }

    /// <summary>
    /// 将文件读取到字符串中
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    /// <param name="encoding">字符编码</param>
    public static string FileToString(string filePath, Encoding encoding) {
        if (!IsExistFile(filePath))
        {
            return string.Empty;
        }
        //创建流读取器
        StreamReader reader = new StreamReader(filePath, encoding);
        try {
            //读取流
            return reader.ReadToEnd();
        }
        catch {
            return string.Empty;
        }
        finally {
            //关闭流读取器
            reader.Close();
        }
    }

    #endregion

    #region 从文件的绝对路径中获取文件名( 包含扩展名 )

    /// <summary>
    /// 从文件的绝对路径中获取文件名( 包含扩展名 )
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static string GetFileName(string filePath) {
        //获取文件的名称
        FileInfo fi = new FileInfo(filePath);
        return fi.Name;
    }

    /// <summary>
    /// 从文件或文件夹的文件名或文件夹名，可能不适用于根目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetName(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            path = path.Replace("\\\\", "/").Replace("\\", "/");
            recheck:
            if (path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
                goto recheck;
            }
            if (path.Contains("/"))
            {
                var last = path.LastIndexOf("/") + 1;
                return path.Substring(last, path.Length - last);
            }
        }

        return path;
    }

    #endregion

    #region 从文件的绝对路径中获取文件名( 不包含扩展名 )

    /// <summary>
    /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static string GetFileNameNoExtension(string filePath) {
        //获取文件的名称
        FileInfo fi = new FileInfo(filePath);
        return fi.Name.Split('.')[0];
    }

    #endregion

    #region 从文件的绝对路径中获取扩展名

    /// <summary>
    /// 从文件的绝对路径中获取扩展名
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>        
    public static string GetExtension(string filePath) {
        //获取文件的名称
        FileInfo fi = new FileInfo(filePath);
        return fi.Extension;
    }

    #endregion

    #region 清空指定目录

    /// <summary>
    /// 清空指定目录下所有文件及子目录,但该目录依然保存.
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    public static void ClearDirectory(string directoryPath) {
        if (IsExistDirectory(directoryPath)) {
            //删除目录中所有的文件
            string[] fileNames = GetFileNames(directoryPath);
            foreach (string t in fileNames) {
                DeleteFile(t);
            }

            //删除目录中所有的子目录
            string[] directoryNames = GetDirectories(directoryPath);
            foreach (string t in directoryNames) {
                DeleteDirectory(t);
            }
        }
    }

    #endregion

    #region 清空文件内容

    /// <summary>
    /// 清空文件内容
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    public static void ClearFile(string filePath) {
        //删除文件
        File.Delete(filePath);

        //重新创建该文件
        CreateFile(filePath);
    }

    #endregion

    #region 删除指定文件

    /// <summary>
    /// 删除指定文件
    /// </summary>
    /// <param name="filePath">文件的绝对路径</param>
    public static void DeleteFile(string filePath) {
        if (IsExistFile(filePath)) {
            File.Delete(filePath);
        }
    }

    #endregion

    #region 删除指定目录

    /// <summary>
    /// 删除指定目录及其所有子目录
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    public static void DeleteDirectory(string directoryPath) {
        if (IsExistDirectory(directoryPath)) {
            Directory.Delete(directoryPath, true);
        }
    }

    #endregion

    //        #region 记录错误日志到文件方法
    //        /// <summary>
    //        /// 记录错误日志到文件方法
    //        /// </summary>
    //        /// <param name="exMessage"></param>
    //        /// <param name="exMethod"></param>
    //        /// <param name="userID"></param>
    //        public static void ErrorLog(string exMessage, string exMethod, int userID) {
    //            try {
    //                string errVir = "/Log/Error/" + DateTime.Now.ToShortDateString() + ".txt";
    //                string errPath = System.Web.HttpContext.Current.Server.MapPath(errVir);
    //                File.AppendAllText(errPath,
    //                                   "{userID:" + userID + ",exMedthod:" + exMethod + ",exMessage:" + exMessage + "}");
    //            }
    //            catch {
    //
    //            }
    //        }
    //        #endregion
    //
    //        #region 输出调试日志
    //        /// <summary>
    //        /// 输出调试日志
    //        /// </summary>
    //        /// <param name="factValue">实际值</param> 
    //        /// <param name="expectValue">预期值</param>
    //        public static void OutDebugLog(object factValue, object expectValue = null) {
    //            string errPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/Log/Debug/{0}.html", DateTime.Now.ToShortDateString()));
    //            if (!Equals(expectValue, null))
    //                File.AppendAllLines(errPath,
    //                                   new[]{string.Format(
    //                                       "【{0}】[{3}] 实际值:<span style='color:blue;'>{1}</span> 预期值: <span style='color:gray;'>{2}</span><br/>",
    //                                       DateTime.Now.ToShortTimeString()
    //                                       , factValue, expectValue, Equals(expectValue, factValue)
    //                                           ? "<span style='color:green;'>成功</span>"
    //                                           : "<span style='color:red;'>失败</span>")});
    //            else
    //                File.AppendAllLines(errPath, new[]{
    //                               string.Format(
    //                                   "【{0}】[{3}] 实际值:<span style='color:blue;'>{1}</span> 预期值: <span style='color:gray;'>{2}</span><br/>",
    //                                   DateTime.Now.ToShortTimeString()
    //                                   , factValue, "空", "<span style='color:green;'>成功</span>")});
    //        }
    //        #endregion

    public static string changePathToAssetDatabasePath(string filePath)
	{
		return filePath.Replace(UnityEngine.Application.dataPath, "Assets").Replace("\\", "/");
	}

    public static string changeAssetDatabasePathToTotalPath(string filePath)
    {
        var dataPath = UnityEngine.Application.dataPath;
        return dataPath.Substring(0, dataPath.LastIndexOf('/')) + "/" + filePath.Replace("\\", "/");
    }
}
