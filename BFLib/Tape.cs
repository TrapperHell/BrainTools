using System.Collections.Generic;

namespace BrainTools
{
    /// <summary>
    /// A class for managing a Brainfuck tape.
    /// </summary>
    public class Tape
    {
        const int MIN_CELL_VALUE = 0, MAX_CELL_VALUE = 255;

        private List<int> content;
        public int Pointer { get; set; }



        public Tape()
        {
            this.content = new List<int>();
            content.Add(0);
            this.Pointer = 0;
        }



        public int Cell
        {
            get
            {
                return this.content[this.Pointer];
            }
            set
            {
                /*
                 * If the assigned value is larger than the byte's size, keep deducting
                 * the max byte size, until the value is smaller. If deducted, finally
                 * reduce the value by one.
                */
                bool mod = false;
                while (value > MAX_CELL_VALUE)
                {
                    value -= MAX_CELL_VALUE;
                    mod = true;
                }

                if (mod)
                    value--;

                /*
                 * If the assigned value is smaller than the zero, keep adding the max
                 * byte size, until the value is greater. If added, finally add the value
                 * by one.
                */
                mod = false;
                while (value < -MAX_CELL_VALUE)
                {
                    value += MAX_CELL_VALUE;
                    mod = true;
                }

                if (mod)
                    value++;

                if (value < MIN_CELL_VALUE)
                    value = (MAX_CELL_VALUE + 1) + value;

                this.content[this.Pointer] = value;
            }
        }

        public List<int> Content
        {
            get
            {
                return this.content;
            }
            set
            {
                if (value == null)
                    this.content.Clear();
                else
                    this.content = value;

                if (this.content.Count == 0)
                    this.content.Add(0);

                if (this.Pointer > this.content.Count)
                    this.Pointer = this.content.Count - 1;
            }
        }

        public void MoveLeft()
        {
            this.Pointer--;
            if (this.Pointer == -1)
            {
                this.content.Insert(0, 0);
                this.Pointer++;
            }
        }

        public void MoveRight()
        {
            this.Pointer++;
            if (this.Pointer == this.content.Count)
                this.content.Add(0);
        }
    }
}