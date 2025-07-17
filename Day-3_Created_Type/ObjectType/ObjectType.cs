namespace CreatedType.ObjectType
{
    public class Stack
    {
        int position;
        object[] data = new object[10];

        public void Push(object obj)
        {
            data[position++] = obj;
        }

        public object Pop()
        {
            return data[--position];
        }
    }

}