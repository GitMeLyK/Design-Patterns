using static System.Console;

namespace DotNetDesignPatternDemos.SOLID.LiskovSubstitutionPrinciple
{

    /*
     * Il principio di Sostituzione. Fondamentalmente si tratta di operare
     * nelle classi applicando questo principio per poter sostituire un tipo
     * base per un sottotipo. Anche se sembra criptico l'esempio porta ad avere
     * un idea chiara del principio.
     * Ipotizzando di avere una classe rettangolo e una classe quadrato e tutte 
     * e due hanno gli attributi per il Width e l'Height della dimensione.
     * Possiamo quindi operare sul rettangolo per ricavare l'area. Adesso però
     * la classe del Quadrato è ereditata dal rettangolo e per ovvietà se 
     * nel codice andiamo a impostare la larghezza che sarà sempre uguale all'altezza
     * essendo un quadrato e stesso se impostiamo la lunghezza sarà definita uguale
     * alla larghezza, questo comportamento per ereditarietà viene sovrscritto quindi
     * sostituito nelle proprietà per sostiutire il comportamento interno.
     * Quindi secondo il principio della Sostituzione si ridfnisce il problema nelle
     * classi derivate sostiuendo i metodi e le proprietà che sono prettamente di
     * quella classe derivata. Quello che è da prestare attenzione è l'uso di override
     * per sostiuire le proprietà ereditate, che altrimenti si riferirebbero al tipo
     * base. Infatti per fare una controprova è bene prestare attenzione a questo passaggio
     * se usiamo definire             
     *      Rectangle sq = new Square();
     * e la classe Square avrebbe usato la parola new per rimpiazzare le proprietà queste
     * ultime sono solo ad uso esclusivo della classe Square e non sovrascritte per la 
     * sostituzione della base e fornirebbero un risultato sbagliato, menttre se usiamo
     * l'overide e calcoliamo l'area allora avremo il risultato corretto perchè la 
     * proprietà viene impostata sulla base della classe a cui si riferisce.
     */

    // using a classic example
    public class Rectangle
    {
        //public int Width { get; set; }
        //public int Height { get; set; }

        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public Rectangle()
        {

        }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        /*
         * con la parola chiave new il metodo o la proprietà sono 
         * ad uso esclusivo del tipo corrente e non seguono il principio
         * della sostituzione
         * 
        public new int Width
        {
          set { base.Width = base.Height = value; }
        }

        public new int Height
        { set { base.Width = base.Height = value; }
        }
        */
        
        /*
         *  Con l'override invece seguaimo il principio della sostituzione
         * e quello che viene assegnato a una nuova istanza di Square tipitizzata
         * alla sua classe base rectangle è richiamabile sulla base ma puntando
         * a questi override di proprietà della classe derivata.
         */

        public override int Width // nasty side effects
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Width = base.Height = value; }
        }
        
    }

    public class Demo
    {
        static public int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            Rectangle rc = new (2, 3);
            WriteLine($"{rc} has area {Area(rc)}");

            /*
             * Grazie al fatto che abbiamo usato l'override quindi alla sostituzione
             * del metodo di base per la classe rectangle il Quadtaro continuerà ad
             * usare i metodi Sovrascritti per calcolare l'area. Difatti il .Width
             * si sta applicando alla classe Square e non alla classe base Rectangle.
             * */
            /*Square*/
            Rectangle sq = new Square();
            sq.Width = 4;
            WriteLine($"{sq} has area {Area(sq)}");
        }
    }
}
