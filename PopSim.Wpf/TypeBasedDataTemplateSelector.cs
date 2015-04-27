using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PopSim.Wpf
{
    public class TypeBasedDataTemplateSelector:DataTemplateSelector
    {
        public TypeBasedDataTemplateSelector()
        {
            TypeDataTemplateMapItemItems = new List<TypeDataTemplateMapItem>();
        }
        public List<TypeDataTemplateMapItem> TypeDataTemplateMapItemItems { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                foreach (var map in TypeDataTemplateMapItemItems)
                {
                    if (item.GetType() == map.Type)
                    {
                        return map.DataTemplate;
                    }
                }
            }
            return base.SelectTemplate(item, container);
        }
    }

    public class TypeDataTemplateMapItem
    {
        public Type Type { get; set; }
        public DataTemplate DataTemplate { get; set; }
    }
}
