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
        List<FrameworkElement> focusedElements;
        int pointer = 0;
        int capacity = 10;
        bool lockUndo = false;

        public UndoRedoController()
        {
            focusedElements = new List<FrameworkElement>();
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
            if (pointer < focusedElements.Count - 1)
            {
                deleteFolowingChanges();
                iteration = false;
            }

            focusedElements.Add(sender);

            if (pointer == capacity - 1)
            {
                focusedElements.RemoveAt(0);
            }
            else if(focusedElements.Count > 1 && iteration)
            {
                pointer++;
            }
        }

        public void Undo()
        {
            if(focusedElements.Count != 0 && !lockUndo && pointer>1)
            {
                focusedElements[pointer].DataContext = false;
                ApplicationCommands.Undo.Execute(null, focusedElements[pointer]);

                if (pointer > 0)
                    pointer--;
                else if(pointer == 0)
                    lockUndo = true;
            }
        }

        public void Redo()
        {
            if (pointer < focusedElements.Count)
            {
                if (pointer < focusedElements.Count - 1)
                    pointer++;
                else if (pointer > 0)
                    lockUndo = false;

                focusedElements[pointer].DataContext = false;
                ApplicationCommands.Redo.Execute(null, focusedElements[pointer]);     
            }
        }

        private void deleteFolowingChanges()
        {
            for(int i = focusedElements.Count - 1; i > pointer ; i--)
            {
                focusedElements.RemoveAt(i);
            }
            pointer = focusedElements.Count;
        }

    }
}
