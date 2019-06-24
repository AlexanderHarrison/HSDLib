﻿using HSDLib.Common;
using System.Collections.Generic;

namespace HSDLib.KAR
{
    public class KAR_GrSplineNode : IHSDNode
    {
        /*public KAR_GrCourseSplineSetup CourseSplineSetup { get; set; }

        public KAR_GrSplineSetup RangeSplineSetup { get; set; }
        public KAR_GrSplineSetup GravitySplineSetup { get; set; }

        public KAR_GrFlowSetup AirFlowSetup { get; set; }
        public KAR_GrFlowSetup ConveyerFlowSetup { get; set; }

        public KAR_GrCourseSpline UnknownGetSplineDataAll { get; set; }
        
        public KAR_GrSplineSetup RailSplineSetup { get; set; }*/
    }
    
    public class KAR_GrCourseSpline : IHSDNode
    {
        public List<HSD_Spline> Paths = new List<HSD_Spline>();

        public override void Open(HSDReader Reader)
        {
            var offset = Reader.ReadUInt32();
            var count = Reader.ReadInt32();

            uint[] offsets = new uint[count];
            Reader.Seek(offset);
            for (int i = 0; i < count; i++)
            {
                offsets[i] = Reader.ReadUInt32();
            }

            for(int i = 0; i < count; i++)
            {
                if (offsets[i] == 0)
                    continue;
                Reader.Seek(offsets[i]);
                var path = new HSD_Spline();
                path.Open(Reader);
                Paths.Add(path);
            }
        }

        public override void Save(HSDWriter Writer)
        {
            foreach(var p in Paths)
            {
                p.Save(Writer);
            }

            Writer.AddObject(Paths);
            foreach (var p in Paths)
            {
                Writer.WritePointer(p);
            }

            if (Paths.Count == 0)
                Writer.Write(0);

            Writer.AddObject(this);
            Writer.WritePointer(Paths);
            Writer.Write(Paths.Count == 0 ? 1 : Paths.Count);
        }
    }


    public class KAR_GrCourseSplineTable : IHSDNode
    {
        public List<int> Indices = new List<int>();

        public override void Open(HSDReader Reader)
        {
            var offset = Reader.ReadUInt32();
            var count = Reader.ReadInt32();
            
            Reader.Seek(offset);
            for (int i = 0; i < count; i++)
            {
                Indices.Add(Reader.ReadInt32());
            }
        }

        public override void Save(HSDWriter Writer)
        {
            Writer.AddObject(Indices);
            foreach (var p in Indices)
            {
                Writer.Write(p);
            }

            Writer.AddObject(this);
            Writer.WritePointer(Indices);
            Writer.Write(Indices.Count);
        }
    }
}
