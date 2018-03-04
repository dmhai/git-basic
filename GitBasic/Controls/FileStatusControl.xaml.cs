﻿using GitBasic.FileSystem;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitBasic.Controls
{
    /// <summary>
    /// Interaction logic for FileStatusControl.xaml
    /// </summary>
    public partial class FileStatusControl : UserControl
    {
        // variable used to hold the item we will be dragging between controls
        Item dragged_item;




        public List<Item> StagedItems
        {
            get { return (List<Item>)GetValue(StagedItemsProperty); }
            set { SetValue(StagedItemsProperty, value); }
        }

        public static readonly DependencyProperty StagedItemsProperty =
            DependencyProperty.Register("StagedItems", typeof(List<Item>), typeof(FileStatusControl), new PropertyMetadata(new List<Item>()));

        public List<Item> UnstagedItems
        {
            get { return (List<Item>)GetValue(UnstagedItemsProperty); }
            set { SetValue(UnstagedItemsProperty, value); }
        }

        public static readonly DependencyProperty UnstagedItemsProperty =
            DependencyProperty.Register("UnstagedItems", typeof(List<Item>), typeof(FileStatusControl), new PropertyMetadata(new List<Item>()));


        public FileStatusControl()
        {
            InitializeComponent();

            //var itemProvider = new ItemProvider();

            //StagedItems = itemProvider.GetItems("C:\\Users\\shaama\\Desktop\\Test Directory", "Staged");
            //UnstagedItems = itemProvider.GetItems("C:\\Users\\shaama\\Desktop\\Test Directory", "Unstaged");

        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {

            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    dragged_item = (Item)Unstaged.SelectedItem;

                    if (dragged_item != null)
                    {
                        DragDropEffects finalDropEffect = DragDrop.DoDragDrop(Unstaged, Unstaged.SelectedValue,
                            DragDropEffects.Move);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                if (dragged_item != null)
                {
                    // No need to modify control, the TreeView will refresh automagically upon repo change
                    Stage_Items(dragged_item);
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());
            }
        }

        private void Stage_Items(Item item)
        {
            List<Item> directory_items;

            // if a DirectoryItem
            if (item is DirectoryItem)
            {
                directory_items = ((DirectoryItem)item).Items;

                Debug.Print(item.Name + " directory added to Staged TreeView");
                // Iterate directory Items
                foreach (Item dir_item in directory_items)
                {
                    // if DirectoryItem, recurse. else stage FileItem
                    if (dir_item is DirectoryItem)
                    {
                        Debug.Print(dir_item.Name + " directory added to Staged TreeView");
                        Stage_Items(dir_item);
                    }
                    else
                    {
                        //Commands.Stage(repo, dir_item.Path);
                        Debug.Print(dir_item.Name + " file added to Staged TreeView");
                    }
                }
            }
            else  // it's a FileItem, stage it
            {

                //Commands.Stage(repo, item.Path);
                Debug.Print(item.Name + " file added to Staged TreeView");
            }
        }
    }
}
