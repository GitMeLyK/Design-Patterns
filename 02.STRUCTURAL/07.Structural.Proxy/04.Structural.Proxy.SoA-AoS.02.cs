using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using static System.Console;


namespace DotNetDesignPatternDemos.Structural.Proxy.SoACompositeProxy
{
    /*
     * Nell'esempio precedente abbiamo visto come un modello composito
     * si comporta nei confronti di un set di proprietà supportate come array
     * adesso invece proviamo una situazione legata alle interfacce utente.
     * Capite molte volte che in una interfaccia si trovino molte caselle di
     * controllo che si vogliono attivare o disattivare e potresti avere anche 
     * diversi raggruppamenti di queste che  sua colta reimposta altre caselle
     * in base al gruppo selezionato. Per fare ciò non è bene pensare a un 
     * modello composito come in precdenza in quanto diventerbbe più complicato
     * il codice. Piuttosto con questo esempio attraverso l'uso corretto di 
     * flag si semplifica il codice e lo si rende più leggibile a parità di prestazioni.
     * Grazie anche alla possibilità di accettare null per bool? in uscita diventa
     * facile utilizzare questi array di stato per una ipotetica chiamata all'uso
     * dei gruppi e identificare se tutto il gruppo è selezionato o meno o se qualcuno
     * nel gruppo è diversamente selezionato rispetto agli altri identificandolo con uno
     * stato di null per All.
     */

    // todo: publish
    public class MasonrySettings
    {
        // Proprietà proxy composita usata non in modo corretto
        //public bool? All
        //{
        //  Stessa cosa succede qui per tutti gli elementi in array composito
        //  che hai per l'interfaccia bisogna ricordardi di fare modifiche a quest
        //  comportamente aggiungeno gli oggetti del gruppo
        //  get
        //  {
        //    if (Pillars == Walls &&
        //        Walls == Floors)
        //      return Pillars;
        //    return null;
        //  }
        //  set
        //  {
        //    Quindi se uno seleziona una singola casella di raggruppamento di controlli
        //    imposta tutte le caselle allo stesso modo, ed essendo un bool? dobbiamo operare
        //    per l'impostazione del valore su .Value di ogni flag
        //    if (!value.HasValue) return;
        //    Pillars = value.Value;
        //    Walls = value.Value;
        //    Floors = value.Value;
        //  } // error-prone!
        //}

        // L'approccio corretto avviene invece definendo
        public bool? All
        {
            get
            {
                // Se il primo flag non è impostato a true o false resttuisce
                // True o False se tutti sono stati impostati uguali rispetto al primo
                if (flags.Skip(1).All(f => f == flags[0]))
                    return flags[0];
                // Se uno qualunque differisce rispetto agli altri restituisce null
                return null;
            }

            set
            {
                if (!value.HasValue) return;
                for (int i = 0; i < flags.Length; ++i)
                    flags[i] = value.Value;
            }
        }

        // Con l'approccio classico avremmo questa situazione
        //public bool Pillars;
        //public bool Walls;
        //public bool Floors;

        // Gruppo di tre flag usati nell'interfaccia definiti in composito con l'array predefinito
        // di tre elementi, e se nel tempo dovesse cambiare l'interfaccia semplicement dovremmo
        // intervenire in questo singolo punto aggiungdo o diminuendo il numero degli elementi
        // per il gruppo nell'interfaccia.
        private readonly bool[] flags = new bool[3];

        /* Ogni flag getter e setter prende dall'indice corrispondente il valore */

        public bool Pillars
        {
            get => flags[0];
            set => flags[0] = value;
        }

        public bool Walls
        {
            get => flags[1];
            set => flags[1] = value;
        }

        public bool Floors
        {
            get => flags[2];
            set => flags[2] = value;
        }

    }
}