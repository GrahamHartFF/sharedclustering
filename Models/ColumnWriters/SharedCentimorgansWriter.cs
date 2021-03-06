﻿using OfficeOpenXml;

namespace AncestryDnaClustering.Models.HierarchicalClustering.ColumnWriters
{
    public class SharedCentimorgansWriter : IColumnWriter
    {
        public string Header => "Shared Centimorgans";
        public bool RotateHeader => true;
        public bool IsAutofit => true;
        public bool IsDecimal => true;
        public double Width => 15;

        public void WriteValue(ExcelRange cell, IClusterableMatch match, LeafNode leafNode) => cell.Value = match.Match.SharedCentimorgans;
    }
}
