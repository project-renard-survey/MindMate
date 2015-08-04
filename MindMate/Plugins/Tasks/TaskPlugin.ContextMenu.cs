﻿using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskPlugin : IPluginMapNodeContextMenu
    {
        private MenuItem[] menuItems;

        private MenuItem SetDueDateMenu { get { return menuItems[0]; } }
        private MenuItem UpdateDueDateMenu { get { return menuItems[1]; } }
        private MenuItem CompleteTaskMenu { get { return menuItems[3]; } }
        private MenuItem RemoveTaskMenu { get { return menuItems[4]; } }

        
        public MenuItem[] CreateContextMenuItemsForNode()
        {
            var t1 = new MenuItem("Set Due Date ...", TaskRes.date_add, SetDueDate_Click);

            var t2 = new MenuItem("Update Due Date ...", TaskRes.date_edit, SetDueDate_Click);

            var t3 = new MenuItem("Quick Due Dates");

            t3.AddDropDownItem(new MenuItem("Today", null, Today_Click));
            t3.AddDropDownItem(new MenuItem("Tomorrow", null, Tomorrow_Click));
            t3.AddDropDownItem(new MenuItem("Next Week", null, NextWeek_Click));
            t3.AddDropDownItem(new MenuItem("Next Month", null, NextMonth_Click));
            t3.AddDropDownItem(new MenuItem("Next Quarter", null, NextQuarter_Click));

            var t4 = new MenuItem("Complete Task", TaskRes.tick, CompleteTask_Click);

            var t5 = new MenuItem("Remove Task", TaskRes.date_delete, RemoveTask_Click);

            menuItems = new MenuItem[] 
            {
                t1,
                t2,
                t3,
                t4,
                t5
            };

            return menuItems;
        }

        public void OnContextMenuOpening(SelectedNodes nodes)
        {
            if (nodes.First.DueDateExists())
            {
                SetDueDateMenu.Visible = false;
                UpdateDueDateMenu.Visible = true;
                RemoveTaskMenu.Visible = true;

            }
            else
            {
                SetDueDateMenu.Visible = true;
                UpdateDueDateMenu.Visible = false;
                RemoveTaskMenu.Visible = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="nodes"></param>
        private void SetDueDate_Click(MenuItem menu, SelectedNodes nodes)
        {
            SetDueDateThroughPicker(nodes);            
        }


        /// <summary>
        /// Should only update the model, all interested views should be updated through the event generated by the model.
        /// </summary>
        /// <param name="node"></param>
        private void SetDueDate(MapNode node)
        {
            // initialize date time picker
            if (node.DueDateExists())
            {
                dateTimePicker.Value = node.GetDueDate();
            }
            else
            {
                dateTimePicker.Value = DateHelper.GetDefaultDueDate();
            }

            // show and set due dates
            if (dateTimePicker.ShowDialog() == DialogResult.OK)
            {
                node.AddTask(dateTimePicker.Value);
            }
        }
        

        /// <summary>
        /// Should only update the model, all interested views should be updated through the event generated by the model.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="nodes"></param>
        private void CompleteTask_Click(MenuItem menu, SelectedNodes nodes)
        {
            for(int i =0; i < nodes.Count; i++)
            {
                MapNode node = nodes[i];

                node.CompleteTask();
            }
        }

        private void RemoveTask_Click(MenuItem menu, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].RemoveTask();
            }
        }

        internal void Today_Click(MenuItem menu, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                SetDueDateToday(nodes[i]);
            }
        }

        internal void Tomorrow_Click(MenuItem menu, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                SetDueDateTomorrow(nodes[i]);
            }
        }

        internal void NextWeek_Click(MenuItem menu, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                SetDueDateNextWeek(nodes[i]);
            }            
        }

        internal void NextMonth_Click(MenuItem menu, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                SetDueDateNextMonth(nodes[i]);
            } 
        }

        internal void NextQuarter_Click(MenuItem menu, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                SetDueDateNextQuarter(nodes[i]);
            }            
        }

        
    }
}
