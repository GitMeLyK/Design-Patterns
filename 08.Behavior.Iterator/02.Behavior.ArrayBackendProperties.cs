using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Iterator.TreeTraversal
{

    /*
     * In questo esempio per definizione del modello Iterator viene costruito
     * un oggetto separato. In questo esempio viene definito uno scenario dove
     * viene rappresentato un albero binario 
     *      A
     *     / \
     *    B   C
     * e ogni elemento è un nodo con un valore rappresentativo e il nodo si trova
     * per la sua posizione ad essere accostato ad un nodo a destra e uno a sinistra
     * nell'albero binario e il suo parent è da dove discende.
     * La classe InOrderIterator<T> è per lo più implementata per come fanno gli altri
     * linguaggi come c++ per usare l'oggetto Iterator e implementa i metodi per attraversare
     * i nodi Mentre usando il framework di c# nella classe Binarytree diventa più semplice
     * leggere ed implementare questo pattern già intrinseco in questo linguaggio. 
     * L'oggetto classico iterator ordinato nella classe InOrderIterator come potrebbe
     * essere in C++ opera sul costrutto definendo quello che possiamo vedere
     *  una macchina a stati dove il reifeirmento corrente è effettivamente lo stato.
     * In C# queste macchine a stati sono create automaticamente quando si definisce 
     * una classe che produce un tipo IEnumerable e usando il metodo implementato IEnumerator
     * è possibile scorrere o interrompere la navigazione dell'insieme come si può vedere nella
     * classe BinaryTree<T> che ha in sè il metodo per attraversare tutta la struttura dell'albero
     * di elementi presenti nell'insieme.
     * Anche se in modo gratutito abbiamo la funzione che serve a scorrere nell'ablero delle
     * strutture non possiamo automaticamente avere il nodo corrente in quanto come per l'oggetto
     * inOrderIterator il MoveNext prima di restiuire la posizione successiva e impostare come
     * corrente la posizione dove si sta andando nella classe BinaryTree<T> perchè questo avvenga
     * la classe deve presentarsi come tipo enumerabile implementanto il metodo automatico GetEnumerator
     * che ritorna il nodo successivo nel contesto dell'iterazione e riportando come nella classe
     * inOrderIterator in c++ il current del nodo corrente da cui a quel punto si possono leggere
     * e valutare tutte le proprietà e i metodi di quel nodo (DuckType).
     */

    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            this.Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            this.Value = value;
            this.Left = left;
            this.Right = right;

            left.Parent = right.Parent = this;
        }
    }

    // Object Iterator c++
    public class InOrderIterator<T>
    {
        public Node<T> Current { get; set; }
        private readonly Node<T> root;
        private bool yieldedStart;

        public InOrderIterator(Node<T> root)
        {
            this.root = Current = root;
            while (Current.Left != null)
                Current = Current.Left;
        }


        public void Reset()
        {
            Current = root;
            yieldedStart = true;
        }

        // La funzione sa come muoversi sul prossimo nodo
        public bool MoveNext()
        {
            if (!yieldedStart)
            {
                yieldedStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                    Current = Current.Left;
                return true;
            }
            else
            {
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }
                Current = p;
                return Current != null;
            }
        }
    }


    // Method Iterator
    // L'albero delle strutture su cui effettuare le operazioni
    // per inserire nodi nel suo contesto e attraverso il framework
    // usare le funzioni già disponibili per l'enumerarione dei nodi
    // e il suo attraversamento.
    public class BinaryTree<T>
    {
        private Node<T> root;

        public BinaryTree(Node<T> root)
        {
            this.root = root;
        }

        // DuckType per restituire lelemento corrente sottoforma
        // di una nuova ulteriore nodo su cui reiterare per l'attraversamento.
        // un pò come il current nel caso della classe InOrderIterator e che
        // grazie a questo avremo nel nodo la possibilità di usare il node.Value
        // per ottenere il riferimento al nodo corrente con le sue proprietà.
        // Rendendo questa classe appunto enumerabile in contesti di foreach
        public InOrderIterator<T> GetEnumerator()
        {
            return new InOrderIterator<T>(root);
        }

        // Il Metodo gratutito offerto da c# per l'enumeratore
        // attraverso il quale iterare nell'albero di strutture in ordine
        public IEnumerable<Node<T>> NaturalInOrder
        {
            get
            {
                // L'implementazione della funzione locale
                IEnumerable<Node<T>> TraverseInOrder(Node<T> current)
                {
                    if (current.Left != null)
                    {
                        foreach (var left in TraverseInOrder(current.Left))
                            yield return left;
                    }
                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in TraverseInOrder(current.Right))
                            yield return right;
                    }
                }
                foreach (var node in TraverseInOrder(root))
                    yield return node;
            }
        }
    }

    public class Demo
    {
        public static void Main()
        {
            //   1
            //  / \
            // 2   3

            // in-order:  213
            // preorder:  123
            // postorder: 231

            var root = new Node<int>(1,
              new Node<int>(2), new Node<int>(3));

            // C++ style
            var it = new InOrderIterator<int>(root);

            while (it.MoveNext())
            {
                Write(it.Current.Value);
                Write(',');
            }
            WriteLine();

            // C# style
            var tree = new BinaryTree<int>(root);

            WriteLine(string.Join(",", tree.NaturalInOrder.Select(x => x.Value)));

            // duck typing!
            foreach (var node in tree)
                WriteLine(node.Value);
        }
    }
}