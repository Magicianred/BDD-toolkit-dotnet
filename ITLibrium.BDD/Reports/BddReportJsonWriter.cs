﻿using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ITLibrium.Bdd.Reports
{
    public class BddReportJsonWriter : BddReportFileWriter
    {
        protected override string Extension => "json";

        public BddReportJsonWriter() { }

        public BddReportJsonWriter(string outputPath) : base(outputPath) { }

        public BddReportJsonWriter(Encoding encoding) : base(encoding) { }

        public BddReportJsonWriter(string outputPath, Encoding encoding) : base(outputPath, encoding) { }

        protected override void Write(IBddReport report, StreamWriter writer)
        {
            string json = JsonConvert.SerializeObject(report);
            writer.Write(json);
        }
    }
}