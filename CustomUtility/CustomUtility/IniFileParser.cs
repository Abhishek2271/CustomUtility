using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
/*
* Description		A simple parser for reading and parsing ini files. 
* Note              All Error Handling should be done outside the parser. Parser only handles errors at basic level.
 *                  Applications calling the parser should handle errors more gracefully. 
*/
namespace CustomUtility
{
    public class IniFileParser
    {
        private Hashtable keyPairs = new Hashtable();
        private string iniFilePath;

        private struct SectionPair
        {
            public string Section;
            public string Key;
        }

        /// <summary>
        /// Opens the INI file at the given path and enumerates the values in the IniParser.
        /// </summary>
        /// <param name="iniPath">Full path to INI file.</param>
        public IniFileParser(string iniPath)
        {
            TextReader iniFile = null;
            string strLine = null;
            string currentRoot = null;
            string[] keyPair = null;

            iniFilePath = iniPath;

            if (File.Exists(iniPath))
            {
                try
                {
                    iniFile = new StreamReader(iniPath);

                    strLine = iniFile.ReadLine();

                    while (strLine != null)
                    {
                        strLine = strLine.Trim();

                        if (strLine.Length > 0 && !strLine.StartsWith("/*") && !strLine.StartsWith("*"))
                        {
                            if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                            {
                                currentRoot = strLine.Substring(1, strLine.Length - 2);
                            }
                            else
                            {
                                keyPair = strLine.Split(new char[] { '=' }, 2);

                                SectionPair sectionPair;
                                string value = null;

                                if (currentRoot == null)
                                    currentRoot = "ROOT";

                                sectionPair.Section = currentRoot.ToUpper().Trim();
                                sectionPair.Key = keyPair[0].ToUpper().Trim();

                                if (keyPair.Length > 1)
                                    value = keyPair[1].Trim();

                                keyPairs.Add(sectionPair, value);
                            }
                        }

                        strLine = iniFile.ReadLine();
                    }

                }
                finally
                {
                    if (iniFile != null)
                        iniFile.Close();
                }
            }
            else
                throw new FileNotFoundException("Unable to locate " + iniPath);

        }



        /// <summary>
        /// Returns the value for the given section, key pair.
        /// </summary>
        /// <param name="sectionName">Section name.</param>
        /// <param name="settingName">Key name.</param>
        public string GetSetting(string sectionName, string settingName)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName.ToUpper().Trim();
            sectionPair.Key = settingName.ToUpper().Trim();
            return (string)keyPairs[sectionPair] ?? "";
        }

        /// <summary>
        /// Enumerates all lines for given section.
        /// </summary>
        /// <param name="sectionName">Section to enum.</param>
        public string[] EnumSection(string sectionName)
        {
            ArrayList tmpArray = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (pair.Section == sectionName)
                    tmpArray.Add(pair.Key);
            }

            return (string[])tmpArray.ToArray(typeof(string));
        }

        /// <summary>
        /// Get all section names.
        /// </summary>
        /// <returns>string array containing section names</returns>
        public string[] GetAllSections()
        {
            ArrayList tmp_array = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (!tmp_array.Contains(pair.Section))
                    tmp_array.Add(pair.Section);
            }
            return (string[])tmp_array.ToArray(typeof(string));
        }
    }
}

