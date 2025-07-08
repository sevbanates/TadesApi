using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Models.CommonModels
{
    public class MenuItem
    {
        public string code { get; set; }
        public string parentcode { get; set; }
        public int actionTotal { get; set; }
        public bool hasChild { get; set; }
        public bool hasPermission { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string[] routerLink { get; set; }
        public string queryParams { get; set; }
        public List<MenuItem> items { get; set; }

        public List<MenuItem> menuData { get; set; }
    }

  

    public class MenuNode
    {
        //    label?: string;
        //data?: T;
        //icon?: string;
        //expandedIcon?: any;
        //collapsedIcon?: any;
        //children?: TreeNode<T>[];
        //leaf?: boolean;
        //expanded?: boolean;
        //type?: string;
        //parent?: TreeNode<T>;
        //partialSelected?: boolean;
        //styleClass?: string;
        //draggable?: boolean;
        //droppable?: boolean;
        //selectable?: boolean;
        //key?: string;

        public string key { get; set; }
        public MenuNode parent { get; set; }
        public bool? selectable { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public List<MenuNode> children { get; set; }
    }
}