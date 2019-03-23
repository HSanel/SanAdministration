using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace San_Administration.Logic
{
    public class UndoRedoController
    {
        List<List<FrameworkElement>> listPlug = new List<List<FrameworkElement>>();
        List<FrameworkElement> focusedElements;
        List<int> pointers = new List<int>();
        //int pointer = 0;
        int capacity = 10;
        bool lockUndo = false;
        int checkedId = 0;
        

        public UndoRedoController(){}

        public void SelectPlugin(int id)
        {
            focusedElements = listPlug[id];
            checkedId = id;
        }

        public List<FrameworkElement> AddPlug
        {
            set
            {
                listPlug.Add(value);
                pointers.Add(0);
            }
        }
        public void ChangeCapacity(int capacity)
        {
            if(capacity <= 50)
            {
                this.capacity = capacity;
            }
        }

        public void Push(FrameworkElement sender)
        {
            bool iteration = true;
            if (pointers[checkedId] < focusedElements.Count - 1)
            {
                deleteFolowingChanges();
                iteration = false;
            }

            focusedElements.Add(sender);

            if (pointers[checkedId] == capacity - 1)
            {
                focusedElements.RemoveAt(0);
            }
            else if(focusedElements.Count > 1 && iteration)
            {
                pointers[checkedId]++;
            }
        }

        public void Undo()
        {
            
            if (focusedElements.Count != 0 && !lockUndo && pointers[checkedId] > 1)
            {
                focusedElements[pointers[checkedId]].DataContext = false;
                ApplicationCommands.Undo.Execute(null, focusedElements[pointers[checkedId]]);

                if (pointers[checkedId] > 0)
                    pointers[checkedId]--;
                else if (pointers[checkedId] == 0)
                    lockUndo = true;
            }
            
        }

        public void Redo()
        {
            if (pointers[checkedId] < focusedElements.Count)
            {
                if (pointers[checkedId] < focusedElements.Count - 1)
                    pointers[checkedId]++;
                else if (pointers[checkedId] > 0)
                    lockUndo = false;

                focusedElements[pointers[checkedId]].DataContext = false;
                ApplicationCommands.Redo.Execute(null, focusedElements[pointers[checkedId]]);
            }
        }

        private void deleteFolowingChanges()
        {
            for(int i = focusedElements.Count - 1; i > pointers[checkedId]; i--)
            {
                focusedElements.RemoveAt(i);
            }
            pointers[checkedId] = focusedElements.Count;
        }

    }
}
