using System;
using System.Threading.Tasks;
using System.Windows;
// using System.Windows.Media;
// using System.Windows.Media.Imaging;

namespace DotNetDesignPatternDemos.Concurrency.SchedulerTask
{
    /*
     *  In questo esempio facciamo uso dello Schedulingask del framework .net
     *  per portare più chiarezza nell'uso contestualizzato a questo framework 
     *  della possibilità offerta come Modello Proposto per questo linguaggio.
     *  
     *  L'esempio seguente crea un'utilità di pianificazione attività personalizzata 
     *  che limita il numero di thread usati dall'app. 
     *  Avvia quindi due set di attività e visualizza informazioni sull'attività e 
     *  sul thread in cui viene eseguita l'attività.
     *  
     *      // Esempio 1
     *  
     *  Un'istanza della TaskScheduler classe rappresenta un'utilità di pianificazione 
     *  delle attività. 
     *  Un'utilità di pianificazione assicura che il lavoro di un'attività viene eseguito.
     *  L'utilità di pianificazione predefinita si basa su pool di thread .NET Framework 4, 
     *  che fornisce l'acquisizione del lavoro per il bilanciamento del carico, 
     *  l'aggiunta/ritiro dei thread per la velocità effettiva massima e buone prestazioni 
     *  complessive. 
     *  Dovrebbe essere sufficiente per la maggior parte degli scenari.
     *  La TaskScheduler classe funge anche da punto di estensione per tutta la logica 
     *  di pianificazione personalizzabile. Sono inclusi meccanismi come pianificare 
     *  un'attività per l'esecuzione e la modalità di esposizione delle attività pianificate 
     *  ai debugger. 
     *  Se sono necessarie funzionalità speciali, è possibile creare un'utilità 
     *  di pianificazione personalizzata e abilitarla per attività o query specifiche.
     *  
     *  Utilità di pianificazione predefinita e pool di thread
     *  
     *  L'utilità di pianificazione predefinita per Task Parallel Library e PLINQ usa il pool 
     *  di thread .NET, rappresentato dalla ThreadPool classe , per accodare ed eseguire il lavoro.
     *  Il pool di thread usa le informazioni fornite dal Task tipo per supportare in modo 
     *  efficiente il parallelismo con granularità fine (unità di lavoro di breve durata) 
     *  che le attività parallele e le query spesso rappresentano.
     *  
     *  Coda globale e code locali
     *  
     *  Il pool di thread gestisce una coda di lavoro FIFO globale (first-in, first-out) 
     *  per i thread in ogni dominio applicazione. Ogni volta che un programma chiama il 
     *  ThreadPool.QueueUserWorkItem metodo (o ThreadPool.UnsafeQueueUserWorkItem), 
     *  il lavoro viene inserito in questa coda condivisa e infine de-accodato nel 
     *  thread successivo che diventa disponibile. 
     *  A partire da .NET Framework 4, questa coda usa un algoritmo senza blocco simile 
     *  alla classe ConcurrentQueue<T>. 
     *  Usando questa implementazione senza blocco, il pool di thread impiega meno 
     *  tempo quando accoda e de-accoda gli elementi di lavoro. 
     *  Questo vantaggio per le prestazioni è disponibile per tutti i programmi che 
     *  usano il pool di thread.
     *  
     *  Le attività di primo livello, ovvero quelle non create nell'ambito di un'altra attività, 
     *  vengono inserite nella coda globale come qualsiasi altro elemento di lavoro. 
     *  Tuttavia le attività annidate o figlio, create nell'ambito di un'altra attività, 
     *  vengono gestite in modo molto diverso. 
     *  Un'attività figlio o annidata viene inserita in una coda locale specifica del thread 
     *  in cui è in esecuzione l'attività padre. 
     *  L'attività padre può essere un'attività di primo livello o anche l'elemento figlio 
     *  di un'altra attività. 
     *  Quando questo thread è disponibile per altro lavoro, esegue innanzitutto una ricerca 
     *  nella coda locale. 
     *  Se sono presenti elementi di lavoro in attesa, è possibile accedervi rapidamente. 
     *  Le code locali sono accessibili nell'ultimo ordine di first-out (LIFO) per 
     *  mantenere la località della cache e ridurre la contesa. 
     *  Per altre informazioni sulle attività figlio e sulle attività annidate, 
     *  vedere Attività figlio collegate e scollegate.
     *  
     *  L'uso delle code locali non solo riduce la pressione sulla coda globale, ma sfrutta 
     *  anche la località dei dati. 
     *  Gli elementi di lavoro nella coda locale fanno spesso riferimento a strutture 
     *  di dati fisicamente vicine tra loro in memoria. 
     *  In questi casi, i dati si trovano già nella cache dopo l'esecuzione della prima 
     *  attività e possono essere accessibili rapidamente. 
     *  Sia PARALLEL LINQ (PLINQ) che la Parallel classe usano attività annidate e attività 
     *  figlio in modo esteso e ottengono velocità significative usando le code di lavoro locali.
     *  
     *  Work theft
     *  A partire da .NET Framework 4, il pool di thread include anche un algoritmo di work Threat 
     *  per assicurarsi che nessun thread sia inattiva mentre altri ancora lavorano nelle code. 
     *  Quando un thread del pool di thread è disponibile per altro lavoro, effettua prima una 
     *  ricerca all'inizio della propria coda locale, quindi nella coda globale e infine nelle 
     *  code locali degli altri thread. 
     *  Se trova un elemento di lavoro nella coda locale di un altro thread, prima applica 
     *  l'euristica per avere la certezza di poter eseguire il lavoro in modo efficiente. 
     *  Se può farlo, rimuove l'elemento di lavoro dalla fine della coda (in ordine FIFO). 
     *  In questo modo viene ridotto il conflitto in ogni coda locale e viene mantenuta la 
     *  località dei dati. Questa architettura consente al pool di thread di bilanciare 
     *  il carico in modo più efficiente rispetto alle versioni precedenti.
     *  
     *  Attività a esecuzione prolungata
     *  Può essere opportuno impedire in modo esplicito l'inserimento di un'attività in una 
     *  coda locale. 
     *  Ad esempio, si potrebbe sapere che un particolare elemento di lavoro verrà eseguito 
     *  per un tempo relativamente lungo e probabilmente bloccherà tutti gli altri elementi 
     *  di lavoro nella coda locale. In questo caso, è possibile specificare l'opzione 
     *  System.Threading.Tasks.TaskCreationOptions, che fornisce all'utilità di pianificazione 
     *  il suggerimento che per l'attività potrebbe essere necessario un altro thread in modo 
     *  che non blocchi l'avanzamento di altri thread o elementi di lavoro nella coda locale. 
     *  Questa opzione consente di evitare completamente il pool di thread, incluse le code 
     *  globali e locali.
     *  
     *  Inlining delle attività
     *  In alcuni casi, quando un Task oggetto è in attesa, può essere eseguito in modo 
     *  sincrono nel thread che esegue l'operazione di attesa. 
     *  Ciò migliora le prestazioni impedendo la necessità di un thread aggiuntivo e usando 
     *  invece il thread esistente, che avrebbe bloccato in caso contrario. 
     *  Per evitare errori dovuti alla reentrancy, l'inlining delle attività si verifica 
     *  solo quando la destinazione di attesa viene trovata nella coda locale del 
     *  thread pertinente.
     *  
     *  Specifica di un contesto di sincronizzazione
     *  È possibile usare il metodo TaskScheduler.FromCurrentSynchronizationContext per 
     *  specificare che è necessario pianificare l'esecuzione di un'attività in un particolare 
     *  thread. 
     *  Ciò risulta utile in framework come Windows Form e Windows Presentation Foundation 
     *  dove l'accesso agli oggetti dell'interfaccia utente è spesso limitato alla coda 
     *  in esecuzione nello stesso thread in cui è stato creato l'oggetto dell'interfaccia utente.
     *  
     *  Nell'esempio 2 seguente viene usato il TaskScheduler.FromCurrentSynchronizationContext 
     *  metodo in un'app Windows Presentation Foundation (WPF) per pianificare un'attività nello 
     *  stesso thread in cui è stato creato il controllo dell'interfaccia utente. 
     *  Nell'esempio viene creato un mosaico di immagini selezionate in modo casuale da 
     *  una directory specificata. Gli oggetti WPF vengono usati per caricare e ridimensionare 
     *  le immagini. I pixel non elaborati vengono quindi passati a un'attività che usa un 
     *  For ciclo per scrivere i dati pixel in una matrice a byte singolo di grandi dimensioni. 
     *  Non è necessaria alcuna sincronizzazione perché non sono presenti due riquadri che occupano 
     *  gli stessi elementi della matrice. 
     *  I riquadri possono anche essere scritti in qualsiasi ordine perché la posizione viene 
     *  calcolata indipendentemente da qualsiasi altro riquadro. La matrice di grandi dimensioni 
     *  viene quindi passata a un'attività eseguita nel thread dell'interfaccia utente, in cui 
     *  i dati pixel vengono caricati in un controllo Immagine.
     *  L'esempio sposta i dati dal thread dell'interfaccia utente, li modifica usando cicli e 
     *  Task oggetti paralleli e quindi li passa a un'attività eseguita nel thread 
     *  dell'interfaccia utente. 
     *  Questo approccio è utile quando è necessario usare Task Parallel Library per eseguire 
     *  operazioni che non sono supportate dall'API WPF o non sono sufficientemente veloci. 
     *  Un altro modo per creare un mosaico di immagini in WPF consiste nell'usare un 
     *  System.Windows.Controls.WrapPanel controllo e aggiungervi immagini. 
     *  Gestisce WrapPanel il lavoro di posizionamento dei riquadri. 
     *  Tuttavia, questo lavoro può essere eseguito solo nel thread dell'interfaccia utente.
     *  
     */


    // Esempio 1


    class Example
    {
       static void Main()
       {
           // Create a scheduler that uses two threads.
           LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(2);
           List<Task> tasks = new List<Task>();

           // Create a TaskFactory and pass it our custom scheduler.
           TaskFactory factory = new TaskFactory(lcts);
           CancellationTokenSource cts = new CancellationTokenSource();

           // Use our factory to run a set of tasks.
           Object lockObj = new Object();
           int outputItem = 0;

           for (int tCtr = 0; tCtr <= 4; tCtr++) {
              int iteration = tCtr;
              Task t = factory.StartNew(() => {
                                           for (int i = 0; i < 1000; i++) {
                                              lock (lockObj) {
                                                 Console.Write("{0} in task t-{1} on thread {2}   ",
                                                               i, iteration, Thread.CurrentThread.ManagedThreadId);
                                                 outputItem++;
                                                 if (outputItem % 3 == 0)
                                                    Console.WriteLine();
                                              }
                                           }
                                        }, cts.Token);
              tasks.Add(t);
          }
          // Use it to run a second set of tasks.
          for (int tCtr = 0; tCtr <= 4; tCtr++) {
             int iteration = tCtr;
             Task t1 = factory.StartNew(() => {
                                           for (int outer = 0; outer <= 10; outer++) {
                                              for (int i = 0x21; i <= 0x7E; i++) {
                                                 lock (lockObj) {
                                                    Console.Write("'{0}' in task t1-{1} on thread {2}   ",
                                                                  Convert.ToChar(i), iteration, Thread.CurrentThread.ManagedThreadId);
                                                    outputItem++;
                                                    if (outputItem % 3 == 0)
                                                       Console.WriteLine();
                                                 }
                                              }
                                           }
                                        }, cts.Token);
             tasks.Add(t1);
          }

          // Wait for the tasks to complete before displaying a completion message.
          Task.WaitAll(tasks.ToArray());
          cts.Dispose();
          Console.WriteLine("\n\nSuccessful completion.");
       }
    }

    // Provides a task scheduler that ensures a maximum concurrency level while
    // running on top of the thread pool.
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
       // Indicates whether the current thread is processing work items.
       [ThreadStatic]
       private static bool _currentThreadIsProcessingItems;

      // The list of tasks to be executed
       private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)

       // The maximum concurrency level allowed by this scheduler.
       private readonly int _maxDegreeOfParallelism;

       // Indicates whether the scheduler is currently processing work items.
       private int _delegatesQueuedOrRunning = 0;

       // Creates a new instance with the specified degree of parallelism.
       public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
       {
           if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
           _maxDegreeOfParallelism = maxDegreeOfParallelism;
       }

       // Queues a task to the scheduler.
       protected sealed override void QueueTask(Task task)
       {
          // Add the task to the list of tasks to be processed.  If there aren't enough
          // delegates currently queued or running to process tasks, schedule another.
           lock (_tasks)
           {
               _tasks.AddLast(task);
               if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
               {
                   ++_delegatesQueuedOrRunning;
                   NotifyThreadPoolOfPendingWork();
               }
           }
       }

       // Inform the ThreadPool that there's work to be executed for this scheduler.
       private void NotifyThreadPoolOfPendingWork()
       {
           ThreadPool.UnsafeQueueUserWorkItem(_ =>
           {
               // Note that the current thread is now processing work items.
               // This is necessary to enable inlining of tasks into this thread.
               _currentThreadIsProcessingItems = true;
               try
               {
                   // Process all available items in the queue.
                   while (true)
                   {
                       Task item;
                       lock (_tasks)
                       {
                           // When there are no more items to be processed,
                           // note that we're done processing, and get out.
                           if (_tasks.Count == 0)
                           {
                               --_delegatesQueuedOrRunning;
                               break;
                           }

                           // Get the next item from the queue
                           item = _tasks.First.Value;
                           _tasks.RemoveFirst();
                       }

                       // Execute the task we pulled out of the queue
                       base.TryExecuteTask(item);
                   }
               }
               // We're done processing items on the current thread
               finally { _currentThreadIsProcessingItems = false; }
           }, null);
       }

       // Attempts to execute the specified task on the current thread.
       protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
       {
           // If this thread isn't already processing a task, we don't support inlining
           if (!_currentThreadIsProcessingItems) return false;

           // If the task was previously queued, remove it from the queue
           if (taskWasPreviouslyQueued)
              // Try to run the task.
              if (TryDequeue(task))
                return base.TryExecuteTask(task);
              else
                 return false;
           else
              return base.TryExecuteTask(task);
       }

       // Attempt to remove a previously scheduled task from the scheduler.
       protected sealed override bool TryDequeue(Task task)
       {
           lock (_tasks) return _tasks.Remove(task);
       }

       // Gets the maximum concurrency level supported by this scheduler.
       public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

       // Gets an enumerable of the tasks currently scheduled on this scheduler.
       protected sealed override IEnumerable<Task> GetScheduledTasks()
       {
           bool lockTaken = false;
           try
           {
               Monitor.TryEnter(_tasks, ref lockTaken);
               if (lockTaken) return _tasks;
               else throw new NotSupportedException();
           }
           finally
           {
               if (lockTaken) Monitor.Exit(_tasks);
           }
       }
    }

    // The following is a portion of the output from a single run of the example:
    //    'T' in task t1-4 on thread 3   'U' in task t1-4 on thread 3   'V' in task t1-4 on thread 3
    //    'W' in task t1-4 on thread 3   'X' in task t1-4 on thread 3   'Y' in task t1-4 on thread 3
    //    'Z' in task t1-4 on thread 3   '[' in task t1-4 on thread 3   '\' in task t1-4 on thread 3
    //    ']' in task t1-4 on thread 3   '^' in task t1-4 on thread 3   '_' in task t1-4 on thread 3
    //    '`' in task t1-4 on thread 3   'a' in task t1-4 on thread 3   'b' in task t1-4 on thread 3
    //    'c' in task t1-4 on thread 3   'd' in task t1-4 on thread 3   'e' in task t1-4 on thread 3
    //    'f' in task t1-4 on thread 3   'g' in task t1-4 on thread 3   'h' in task t1-4 on thread 3
    //    'i' in task t1-4 on thread 3   'j' in task t1-4 on thread 3   'k' in task t1-4 on thread 3
    //    'l' in task t1-4 on thread 3   'm' in task t1-4 on thread 3   'n' in task t1-4 on thread 3
    //    'o' in task t1-4 on thread 3   'p' in task t1-4 on thread 3   ']' in task t1-2 on thread 4
    //    '^' in task t1-2 on thread 4   '_' in task t1-2 on thread 4   '`' in task t1-2 on thread 4
    //    'a' in task t1-2 on thread 4   'b' in task t1-2 on thread 4   'c' in task t1-2 on thread 4
    //    'd' in task t1-2 on thread 4   'e' in task t1-2 on thread 4   'f' in task t1-2 on thread 4
    //    'g' in task t1-2 on thread 4   'h' in task t1-2 on thread 4   'i' in task t1-2 on thread 4
    //    'j' in task t1-2 on thread 4   'k' in task t1-2 on thread 4   'l' in task t1-2 on thread 4
    //    'm' in task t1-2 on thread 4   'n' in task t1-2 on thread 4   'o' in task t1-2 on thread 4
    //    'p' in task t1-2 on thread 4   'q' in task t1-2 on thread 4   'r' in task t1-2 on thread 4
    //    's' in task t1-2 on thread 4   't' in task t1-2 on thread 4   'u' in task t1-2 on thread 4
    //    'v' in task t1-2 on thread 4   'w' in task t1-2 on thread 4   'x' in task t1-2 on thread 4
    //    'y' in task t1-2 on thread 4   'z' in task t1-2 on thread 4   '{' in task t1-2 on thread 4
    //    '|' in task t1-2 on thread 4   '}' in task t1-2 on thread 4   '~' in task t1-2 on thread 4
    //    'q' in task t1-4 on thread 3   'r' in task t1-4 on thread 3   's' in task t1-4 on thread 3
    //    't' in task t1-4 on thread 3   'u' in task t1-4 on thread 3   'v' in task t1-4 on thread 3
    //    'w' in task t1-4 on thread 3   'x' in task t1-4 on thread 3   'y' in task t1-4 on thread 3
    //    'z' in task t1-4 on thread 3   '{' in task t1-4 on thread 3   '|' in task t1-4 on thread 3


    // Esempio 2 Solo per .net non core
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /*
    public partial class MainWindow : Window
    {
        private int fileCount;
        int colCount;
        int rowCount;
        private int tilePixelHeight;
        private int tilePixelWidth;
        private int largeImagePixelHeight;
        private int largeImagePixelWidth;
        private int largeImageStride;
        PixelFormat format;
        BitmapPalette palette = null;

        public MainWindow()
        {
            InitializeComponent();

            // For this example, values are hard-coded to a mosaic of 8x8 tiles.
            // Each tile is 50 pixels high and 66 pixels wide and 32 bits per pixel.
            colCount = 12;
            rowCount = 8;
            tilePixelHeight = 50;
            tilePixelWidth = 66;
            largeImagePixelHeight = tilePixelHeight * rowCount;
            largeImagePixelWidth = tilePixelWidth * colCount;
            largeImageStride = largeImagePixelWidth * (32 / 8);
            this.Width = largeImagePixelWidth + 40;
            image.Width = largeImagePixelWidth;
            image.Height = largeImagePixelHeight;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            // For best results use 1024 x 768 jpg files at 32bpp.
            string[] files = System.IO.Directory.GetFiles(@"C:\Users\Public\Pictures\Sample Pictures\", "*.jpg");

            fileCount = files.Length;
            Task<byte[]>[] images = new Task<byte[]>[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                int x = i;
                images[x] = Task.Factory.StartNew(() => LoadImage(files[x]));
            }

            // When they've all been loaded, tile them into a single byte array.
            var tiledImage = Task.Factory.ContinueWhenAll(
                images, (i) => TileImages(i));

            // We are currently on the UI thread. Save the sync context and pass it to
            // the next task so that it can access the UI control "image".
            var UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();

            // On the UI thread, put the bytes into a bitmap and
            // display it in the Image control.
            var t3 = tiledImage.ContinueWith((antecedent) =>
            {
                // Get System DPI.
                Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow)
                                            .CompositionTarget.TransformToDevice;
                double dpiX = m.M11;
                double dpiY = m.M22;

                BitmapSource bms = BitmapSource.Create(largeImagePixelWidth,
                    largeImagePixelHeight,
                    dpiX,
                    dpiY,
                    format,
                    palette, //use default palette
                    antecedent.Result,
                    largeImageStride);
                image.Source = bms;
            }, UISyncContext);
        }

        byte[] LoadImage(string filename)
        {
            // Use the WPF BitmapImage class to load and
            // resize the bitmap. NOTE: Only 32bpp formats are supported correctly.
            // Support for additional color formats is left as an exercise
            // for the reader. For more information, see documentation for ColorConvertedBitmap.

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filename);
            bitmapImage.DecodePixelHeight = tilePixelHeight;
            bitmapImage.DecodePixelWidth = tilePixelWidth;
            bitmapImage.EndInit();

            format = bitmapImage.Format;
            int size = (int)(bitmapImage.Height * bitmapImage.Width);
            int stride = (int)bitmapImage.Width * 4;
            byte[] dest = new byte[stride * tilePixelHeight];

            bitmapImage.CopyPixels(dest, stride, 0);

            return dest;
        }

        int Stride(int pixelWidth, int bitsPerPixel)
        {
            return (((pixelWidth * bitsPerPixel + 31) / 32) * 4);
        }

        // Map the individual image tiles to the large image
        // in parallel. Any kind of raw image manipulation can be
        // done here because we are not attempting to access any
        // WPF controls from multiple threads.
        byte[] TileImages(Task<byte[]>[] sourceImages)
        {
            byte[] largeImage = new byte[largeImagePixelHeight * largeImageStride];
            int tileImageStride = tilePixelWidth * 4; // hard coded to 32bpp

            Random rand = new Random();
            Parallel.For(0, rowCount * colCount, (i) =>
            {
                // Pick one of the images at random for this tile.
                int cur = rand.Next(0, sourceImages.Length);
                byte[] pixels = sourceImages[cur].Result;

                // Get the starting index for this tile.
                int row = i / colCount;
                int col = (int)(i % colCount);
                int idx = ((row * (largeImageStride * tilePixelHeight)) + (col * tileImageStride));

                // Write the pixels for the current tile. The pixels are not contiguous
                // in the array, therefore we have to advance the index by the image stride
                // (minus the stride of the tile) for each scanline of the tile.
                int tileImageIndex = 0;
                for (int j = 0; j < tilePixelHeight; j++)
                {
                    // Write the next scanline for this tile.
                    for (int k = 0; k < tileImageStride; k++)
                    {
                        largeImage[idx++] = pixels[tileImageIndex++];
                    }
                    // Advance to the beginning of the next scanline.
                    idx += largeImageStride - tileImageStride;
                }
            });
            return largeImage;
        }
    }
    */

    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
