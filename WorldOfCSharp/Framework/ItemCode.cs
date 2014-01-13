using System;

namespace WorldOfCSharp
{
    public struct ItemCode
    {
        private int baseTypeInt;
        private int subTypeInt;

        public ItemCode(int baseType, int subType)
        {
            this.baseTypeInt = baseType;
            this.subTypeInt = subType;
        }

        public int BaseTypeInt
        {
            get { return this.baseTypeInt; }
            set
            {
                if (baseTypeInt >= 0 && baseTypeInt <= ItemType.ItemTypeArr.Length)
                    this.baseTypeInt = value;
                else
                    throw new ArgumentOutOfRangeException("Item base type is out of range.");
            }
        }

        public int SubTypeInt
        {
            get { return this.subTypeInt; }
            set
            {
                if (subTypeInt >= 0 && subTypeInt <= ItemType.ItemTypeArr[BaseTypeInt].Length)
                    this.subTypeInt = value;
                else
                    throw new ArgumentOutOfRangeException("Item sub type is out of range.");
            }
        }
    }
}
