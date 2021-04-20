namespace PinePhoneCore.Helpers
{
    public struct Rectangle
    {
        public int X,Y,W,H;
        private int X2,Y2;

        public Rectangle(int x,int y,int w, int h)
        {
            X=x;
            W=w;
            X2=X+W;
            Y=y;
            H=h;
            Y2=Y+H;
        }
        public bool Contains(int x, int y) 
        {
            if(x >= X)
                if(x <= X2)
                    if(y >= Y)
                        if(y <= Y2)
                            return true;
            return false;
        } 
    }
}