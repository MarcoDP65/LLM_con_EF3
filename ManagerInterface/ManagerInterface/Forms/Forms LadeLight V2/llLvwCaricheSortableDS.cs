using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace ChargerLogic
{
    public class llLvwCaricheSortableDS : FastObjectListDataSource
    {
        private OLVColumn sortColumn;
        public llLvwCaricheSortableDS(FastObjectListView listView) : base(listView) { }

        public OLVColumn SortColumn
        {
            get { return this.sortColumn; }
            set { this.sortColumn = value; }
        }

        public override void Sort(OLVColumn column, SortOrder sortOrder)
        {
            if (sortOrder != SortOrder.None)
            {
                ArrayList objects = (ArrayList)this.listView.Objects;
                objects.Sort(new ModelObjectComparer(this.SortColumn, SortOrder.Ascending, column, sortOrder));

            }
            this.RebuildIndexMap();
        }
    }
}
