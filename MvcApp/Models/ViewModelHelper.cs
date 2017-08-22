using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farm.Authority.DataContext;
using Farm.Raisers.DataContext;

namespace MvcApp.UI
{
    public static class ViewModelHelper
    {
        public static List<SelectListItem> GetSalesMannerItems(string defValue,bool hasAll=false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();

            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            Items.Add(new SelectListItem { Text = "内销", Value = "内销", Selected = (defValue == "内销") });
            Items.Add(new SelectListItem { Text = "优鲜", Value = "优鲜", Selected = (defValue == "优鲜") });
            Items.Add(new SelectListItem { Text = "出口", Value = "出口", Selected = (defValue == "出口") });
            Items.Add(new SelectListItem { Text = "本地", Value = "本地", Selected = (defValue == "本地") });
            Items.Add(new SelectListItem { Text = "晨丰", Value = "晨丰", Selected = (defValue == "晨丰") });
            Items.Add(new SelectListItem { Text = "广联", Value = "广联", Selected = (defValue == "广联") });
            Items.Add(new SelectListItem { Text = "处理猪", Value = "处理猪", Selected = (defValue == "处理猪") });
            return Items;
        }

        public static List<SelectListItem> GetSalesGradeItems(string defValue, bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            Items.Add(new SelectListItem { Text = "正品", Value = "正品", Selected = (defValue == "正品") });
            Items.Add(new SelectListItem { Text = "次品", Value = "次品", Selected = (defValue == "次品") });
            Items.Add(new SelectListItem { Text = "极外品", Value = "极外品", Selected = (defValue == "极外品") });
            return Items;
        }

        public static List<SelectListItem> GetRaiserStatusItems(bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            Items.Add(new SelectListItem { Text = "空闲", Value = "空闲" });
            Items.Add(new SelectListItem { Text = "待调", Value = "待调" });
            Items.Add(new SelectListItem { Text = "在养", Value = "在养" });
            Items.Add(new SelectListItem { Text = "淘汰", Value = "淘汰" });
            return Items;
        }

        public static List<SelectListItem> GetRaiserStatusItems(string defValue, bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            Items.Add(new SelectListItem { Text = "空闲", Value = "3",Selected  = (defValue == "3") });
            Items.Add(new SelectListItem { Text = "待调", Value = "1", Selected = (defValue == "1") });
            Items.Add(new SelectListItem { Text = "在养", Value = "2", Selected = (defValue == "2") });
            Items.Add(new SelectListItem { Text = "淘汰", Value = "0", Selected = (defValue == "0") });
            return Items;
        }

        public static List<SelectListItem> GetPactStatusItems(string defValue, bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            Items.Add(new SelectListItem { Text = "待调", Value = "0", Selected = (defValue == "0") });
            Items.Add(new SelectListItem { Text = "已调", Value = "1", Selected = (defValue == "1") });
            return Items;
        }

        public static List<SelectListItem> GetRoleGroup(int select = 0, bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            var model = new AuthorityRepository().GetEntities<tbRole>(p=>!p.disabled);

            foreach (var m in model)
            {
                SelectListItem item = new SelectListItem();
                item.Text = m.name;
                item.Value = m.ID.ToString();
                item.Selected = (m.ID == select);
                Items.Add(item);
            }
            return Items;
        }

        
        public static List<SelectListItem> GetAuthoryFieldItems(bool hasAll=false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if(hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "", Selected = true });

            var db = new FarmRepository();
            
            foreach (var f in db.GetEntities<Structure>(p=>p.typeID==1))
            {
                SelectListItem item = new SelectListItem();
                item.Text = f.name;
                item.Value = f.ID.ToString();
                Items.Add(item);
            }
            return Items;
        }

        public static List<SelectListItem> GetAuthoryFieldItems(string defValue, bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            var db = new FarmRepository();
            foreach (var f in db.GetEntities<Structure>(p => p.typeID == 1))
            {
                Items.Add(new SelectListItem { 
                    Text = f.name, Value = f.ID.ToString(), Selected = (f.ID.ToString() == defValue) });
            }
            return Items;
        }

        
        /*
        public static List<SelectListItem> GetPigStatusItems(string defValue, bool hasAll = false)
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            if (hasAll)
                Items.Add(new SelectListItem { Text = "全部", Value = "" });

            foreach (var item in Pig.DctStatus)
            {
                Items.Add(new SelectListItem { Text = item.Value, Value = item.Key.ToString(), Selected = (item.Key.ToString() == defValue) });
            }
            return Items;
        }*/
    }
}