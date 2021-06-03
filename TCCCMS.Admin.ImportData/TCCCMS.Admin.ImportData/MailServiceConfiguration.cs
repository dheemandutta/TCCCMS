using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace TCCCMS.Admin.ImportData
{
    public class MailServiceConfiguration
    {

        public int Port { get; set; }
        public string MailServerDomain { get; set; }
        public string MailId { get; set; }
        public string MailPassword { get; set; }
        public string MailServerType { get; set; }
        public string AttachmentPath { get; set; }
        public string SubjectLine { get; set; }


        /// <summary>
        /// Loads the configuration file from the provided location
        /// </summary>
        /// <param name="configFilePath">Full path of the configuration file along with file name</param>
        /// <returns></returns>
        public static MailServiceConfiguration Load(string configFilePath)
        {
            MailServiceConfiguration _returnValue = new MailServiceConfiguration();
            var serializer = new XmlSerializer(typeof(MailServiceConfiguration));

            try
            {
                using (var reader = new StreamReader(configFilePath))
                {
                    _returnValue = (MailServiceConfiguration)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.EventLog, ex.Message);
                return new MailServiceConfiguration();
            }
            return _returnValue;
        }

        /// <summary>
        /// Saves the configuration file to the provide location
        /// </summary>
        /// <param name="configFilePath">Full path of the configuration file along with file name</param>
        public void Save(string configFilePath)
        {

            var serializer = new XmlSerializer(typeof(MailServiceConfiguration));

            try
            {
                using (var writer = new StreamWriter(configFilePath))
                {
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.EventLog, ex.Message);
            }
        }
    }
}
