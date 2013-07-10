﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace erdx2sql
{
    struct SharedData
    {
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string primaryKeys { get; set; }
        public bool IsAutoIncrement { get; set; }
    }

    /// <summary>
    /// eXERD의 erdx파일을 분석해 생성쿼리를 만드는 유틸리티
    /// 현재 버젼은 sqlite만 지원한다.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // 헤더 출력
            PrintHeader();

            // 인자가 없으면 사용법 출력
            if (args.Length == 0)
            {
                PrintUsage();
                return;
            }

            // 해당 파일이 존재하는지 확인
            var file = new FileInfo(args[0]);
            var result = CheckFile(file);
            if (result == false)
                return;

            // DOM을 이용해 생성 쿼리를 만든다.
            string xml = ReadXml(file);
            xml = xml.Replace("version=\"1.1\"", "version=\"1.0\"");        //  버젼 강제로 치환
            var dom = new XmlDocument();
            dom.LoadXml(xml);
            var output = new StringBuilder();
            var sharedData = new SharedData();
            ParseNode(dom, output, ref sharedData);

            // 만든 생성 쿼리를 파일로 저장한다.
            if (args.Length == 1)
            {
                Console.WriteLine(output);
                return;
            }

            var outputFilename = args[1];
            var outputFile = new FileInfo(outputFilename);
            var sw = outputFile.CreateText();
            sw.Write(output);
            sw.Close();
            Console.WriteLine("출력: " + outputFile.FullName);
        }

        private static string ReadXml(FileInfo file)
        {
            var tr = new StreamReader(file.OpenRead());
            var result = tr.ReadToEnd();
            tr.Close();

            return result;
        }

        private static void ParseNode(XmlNode pnode, StringBuilder output, ref SharedData sharedData)
        {
            string comma = " ";
            foreach (XmlNode node in pnode.ChildNodes)
            {
                // 전처리
                if (node.Name == "tables")
                {
                    string tableName = node.Attributes["physicalName"].Value;
                    string logicalName = node.Attributes["logicalName"].Value;
                    var f = @"
-- {1}
DROP TABLE {0};
CREATE TABLE {0} (";
                    output.AppendFormat(f, tableName, logicalName);
                }
                if (node.Name == "columns")
                {
                }

                if (node.HasChildNodes == true)
                    ParseNode(node, output, ref sharedData);
                
                // 후처리
                if (node.Name == "tables")
                {
                    if (sharedData.primaryKeys != "")
                        output.AppendLine().Append("   , PRIMARY KEY(" + sharedData.primaryKeys + ")");
                    output.AppendLine().Append(");");
                    sharedData = new SharedData();
                }
                if (node.Name == "columns")
                {
                    string columnName = node.Attributes["physicalName"].Value;
                    string logicalName = node.Attributes["logicalName"].Value;
                    string notnull = "";
                    if (sharedData.IsPrimaryKey == true)
                    {
                        sharedData.primaryKeys += (string.IsNullOrEmpty(sharedData.primaryKeys) == true ? "" : ", ") + columnName;
                        sharedData.IsPrimaryKey = false;
                        notnull = "NOT NULL";
                    }
                    if (sharedData.IsAutoIncrement == true)
                    {
                        sharedData.primaryKeys += " AUTOINCREMENT";
                        sharedData.IsAutoIncrement = false;
                    }
                    
                    notnull = (node.Attributes["notNull"] == null || node.Attributes["notNull"].Value == "FALSE") && notnull == "" ? "" : " NOT NULL";

                    var domainNode = GetDomainNode(pnode.OwnerDocument, node.Attributes["domain"].Value);
                    var type = domainNode.Attributes["suggestedDataType"];
                    if (type == null)
                        type = domainNode.ParentNode.Attributes["suggestedDataType"];
                    
                    var defaultValue = domainNode.Attributes["suggestedDefaultValue"];
                    string @default = "";
                    if (defaultValue == null)
                        defaultValue = domainNode.ParentNode.Attributes["suggestedDefaultValue"];
                    if (defaultValue != null)
                        @default = " DEFAULT '" + defaultValue.Value + "'";
                    var f = @"
   {0} {1} {2}{3}{4}     -- {5}";
                    output.AppendFormat(f, comma, columnName, type.Value, notnull, @default, logicalName);
                    comma = ",";
                }
                if (node.Name == "value" && node.ParentNode.Name == "memberships")
                {
                    var type = node.Attributes["xsi:type"].Value;
                    if (type == "e:PrimaryKeyMembership")
                        sharedData.IsPrimaryKey = true;
                }
                if (node.Name == "identity")
                {
                    //var startWith = node.Attributes["startWith"].Value;
                    //var incrementBy = node.Attributes["incrementBy"].Value;
                    sharedData.IsAutoIncrement = true;
                }
            }
        }

        private static bool CheckFile(FileInfo file)
        {
            Console.WriteLine("파일명: " + file.FullName);
            if (file.Extension != ".erdx")
            {
                Console.WriteLine("erdx 파일이 아닙니다.");
                return false;
            }
            if (file.Exists == false)
            {
                Console.WriteLine("파일이 존재하지 않습니다.");
                return false;
            }

            return true;
        }

        private static void PrintHeader()
        {
            Console.WriteLine("erdx2sql Ver. 0.1 copyright(c) (주)데브넥트");
        }
        
        private static void PrintUsage()
        {
            Console.WriteLine("사용법: erdx2sql filename.erdx [output.sql]");
        }

        private static XmlNode GetDomainNode(XmlDocument root, string path)
        {
            // //@configuration/@domainDefinition/@domain.2/@domain.1
            // -> 으로 변경해야 한다.
            // //configuration/domainDefinition/domain[3]/domain[2]

            path = path.Replace("@", "");
            path = Regex.Replace(path, @"\.(\d+)", m => "[" + (int.Parse(m.Groups[1].Value) + 1) +"]");

            return root.SelectSingleNode(path);
        }
    }
}
