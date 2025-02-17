﻿//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpFileInfo.cs">(c) 2017 Mike Fourie and Contributors (https://github.com/mikefourie/MSBuildExtensionPack) under MIT License. See https://opensource.org/licenses/MIT </copyright>
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace MSBuild.ExtensionPack.Communication.Extended
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <c>FtpFileInfo</c> class encapsulates a remote FTP directory.
    /// </summary>
    [Serializable]
    public sealed class FtpFileInfo : FileSystemInfo
    {
        private readonly string fileName; 
        private readonly FtpConnection ftpConnection;

        private DateTime? lastAccessTime;
        private DateTime? lastWriteTime;
        private DateTime? creationTime;

        public FtpFileInfo(FtpConnection ftp, string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            this.OriginalPath = filePath;
            this.FullPath = filePath;
            this.ftpConnection = ftp;
            this.fileName = Path.GetFileName(filePath);
        }

        private FtpFileInfo(SerializationInfo info, StreamingContext context) : base(info, context)
        {         
        }

        public FtpConnection FtpConnection => this.ftpConnection;

        public new DateTime? LastAccessTime
        {
            get { return this.lastAccessTime.HasValue ? (DateTime?)this.lastAccessTime.Value : null; }
            internal set => this.lastAccessTime = value;
        }

        public new DateTime? CreationTime
        {
            get => this.creationTime.HasValue ? (DateTime?)this.creationTime.Value : null;
            internal set => this.creationTime = value;
        }

        public new DateTime? LastWriteTime
        {
            get => this.lastWriteTime.HasValue ? (DateTime?)this.lastWriteTime.Value : null;
            internal set => this.lastWriteTime = value;
        }

        public new DateTime? LastAccessTimeUtc => this.lastAccessTime?.ToUniversalTime();

        public new DateTime? CreationTimeUtc => this.creationTime?.ToUniversalTime();

        public new DateTime? LastWriteTimeUtc => this.lastWriteTime?.ToUniversalTime();

        public new FileAttributes Attributes { get; internal set; }

        public override string Name => this.fileName;

        public override bool Exists => this.FtpConnection.FileExists(this.FullName);

        public override void Delete()
        {
            this.FtpConnection.DeleteDirectory(this.FullName);
        }

        /// <summary>
        /// No specific impelementation is needed of the GetObjectData to serialize this object
        /// because all attributes are redefined.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data. </param>
        /// <param name="context">The destination for this serialization. </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}