using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Ticket.AsyncEnterpriseReports.Core.Configuration.Section
{
    public class ProcessManagerConfigurationSection : ConfigurationSection
    {
        //[ConfigurationProperty("logLevel")]
        //public String LogLevel
        //{
        //    get
        //    {
        //        return (String)this["logLevel"];
        //    }
        //    set
        //    {
        //        this["logLevel"] = value;
        //    }
        //}

        [ConfigurationProperty("executors")]
        public ProcessExecutorCollection Executors
        {
            get
            {
                return (ProcessExecutorCollection)this["executors"];
            }
        }

        [ConfigurationProperty("fileSystemRepository", IsRequired = true)]
        public FileSystemRepositoryConfig FileSystemRepository
        {
            get
            {
                return (FileSystemRepositoryConfig)this["fileSystemRepository"];
            }
        }

        [ConfigurationProperty("queueResponseSender", IsRequired = true)]
        public QueueSenderConfig QueueResponseSender
        {
            get
            {
                return (QueueSenderConfig)this["queueResponseSender"];
            }
        }


        [ConfigurationProperty("reportRepository", IsRequired = true)]
        public RepositoryConfig ReportRepository
        {
            get
            {
                return (RepositoryConfig)this["reportRepository"];
            }
        }

        [ConfigurationProperty("ftpRepository", IsRequired = false)]
        public FTPRepositoryConfig FTPRepository
        {
            get
            {
                return (FTPRepositoryConfig)this["ftpRepository"];
            }
        }
             
    }
}
