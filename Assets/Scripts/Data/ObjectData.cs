using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class ObjectData
    {
        public int object_id;
        public float x;
        public float y;
        public float z;
        public Header header;
        public List<Point> contour_points;
        public int bin_id;
        public int n_objects;
        public float object_area;
        public float total_area;
        public float velocity;
    }
}