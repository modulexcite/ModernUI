using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAProject
{
    class Tree
    {
        private List<Leaf> leaf = new List<Leaf>();

        public Tree()
        {
            leaf.Add(new Leaf()
            {
                Question = "Height",
                Left = "1.79m o menos",
                Right = "1.80m o mas",
                Step = 0
            });

            leaf.Add(new Leaf()
            {
                Question = "Weight",
                Left = "219lb o menos",
                Right = "220lb o mas",
                Step = 1
            });

            leaf.Add(new Leaf()
            {
                Question = "1st",
                Left = "29 o menos",
                Right = "30 o mas",
                Step = 2
            });

            leaf.Add(new Leaf()
            {
                Question = "Att",
                Left = "99 o menos",
                Right = "100 o mas",
                Step = 3
            });

            leaf.Add(new Leaf()
            {
                Question = "Yds/G",
                Left = "39 o menos",
                Right = "40 o mas",
                Step = 4
            });

            leaf.Add(new Leaf()
            {
                Question = "Avg",
                Left = "3 o menos",
                Right = "4 o mas",
                Step = 5
            });

            leaf.Add(new Leaf()
            {
                Question = "Lng",
                Left = "39 o menos",
                Right = "40 o mas",
                Step = 6
            });
        }

        public Leaf GetLeaf(int step)
        {
            return leaf[step];
        }

        public string LeafQuestion(int step)
        {
            return leaf[step].Question;
        }

        public string LeafLeft(int step)
        {
            return leaf[step].Left;
        }

        public string LeafRight(int step)
        {
            return leaf[step].Right;
        }

        public int LeafStep(int step)
        {
            return leaf[step].Step;
        }
            
    }
}
