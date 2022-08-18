using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.DTO
{
   public  class JsTreeModel
    {
        public string id { get; set; }

        public string text { get; set; }

        public string icon { get; set; }
        public JsTreeModelstate state { get; set; }
        public object li_attr { get; set; }            // attributes for the generated LI node
        public string a_attr { get; set; }           // attributes for the generated A node
    }  
    public class JsTreeModelByParent: JsTreeModel
    {
        public string parent { get; set; }
    }
    public class JsTreeModelByChildren: JsTreeModel
    {
        public List<JsTreeModelByChildren> children { get; set; } // array of strings or objects
    }
}
