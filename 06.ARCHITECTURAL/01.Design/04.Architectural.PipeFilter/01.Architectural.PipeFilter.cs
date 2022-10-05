using System;

namespace DotNetDesignPatternDemos.Architectural.PipeFilter
{

    /*
     *  Il modello architettonico pipe-filter è molto comune e utilizzato per elaborare i dati. 
     *  È flessibile e scalabile. Questo modello è esistito in altre aree. 
     *  Tuttavia, è la soluzione perfetta per i problemi software che richiedono passaggi o filtri.
     *  
     *  Definizione della serie pipe-filter
     *  Pensa a un inizio e a un endpoint. 
     *  I dati devono andare dall'inizio alla fine e il lavoro sarà fatto su di esso lungo 
     *  la strada. Il lavoro svolto sono i filtri e possiamo posizionare qualsiasi numero 
     *  di filtri lungo il viaggio. Ognuno è autonomo in un certo senso in quanto prende 
     *  i dati, fa il lavoro e poi lo sputa dall'altra parte. 
     *  Possiamo cambiare innumerevoli passaggi come questo insieme come si potrebbe fare 
     *  con i blocchi Lego.
     *  
     *  Ogni passo un obiettivo
     *  Uno dei modi migliori per creare una soluzione facile da mantenere e scalabile 
     *  è eseguire passaggi ben definiti che possono essere facilmente sostituiti. 
     *  Possiamo anche aggiungere passaggi che bilanciano il carico o forniscono altri 
     *  miglioramenti delle prestazioni tra di loro. 
     *  Questa è la forza del modello pipe-filter. 
     *  Possiamo aggiungere, rimuovere e aggiornare i filtri con facilità senza ricostruire 
     *  il sistema. Abbiamo input e output ben definiti e coerenti quando ogni 
     *  passaggio inserisce i dati, esegue l'elaborazione e poi li sputa fuori. 
     *  Quegli stessi ingressi e uscite da un passo all'altro rendono ogni filtro intercambiabile. 
     *  Allo stesso modo, possiamo iniziare semplicemente con un singolo filtro, testarlo 
     *  e quindi passare a una serie di filtri più complessa. 
     *  Ancora meglio, possiamo sempre ridurre la soluzione a un singolo filtro e convalidarla.
     *  
     *  Sfide
     *  Il modello pipe-filter presenta inconvenienti legati ai punti di forza. 
     *  Ad esempio, potrebbero esserci prerequisiti in una soluzione che limitano 
     *  l'intercambiabilità. Allo stesso modo, un gruppo di filtri può entrare in 
     *  conflitto o ridurre le prestazioni. Pertanto, dobbiamo essere saggi nel 
     *  nostro design e non ricorrere all'aggiunta di più filtri. 
     *  Qualsiasi soluzione può essere sovra-architettata. 
     *  Pertanto, sii intenzionale nel progettare sia i filtri che il flusso. 
     *  Un buon design fa la giusta quantità di lavoro ad ogni passo. 
     *  Anche questo lavoro non richiede ipotesi sui dati che sta elaborando.
     *  
    */

    /*
     * Più nel dettaglio con un esempio
     * 
     * Una pipe è una coda di messaggi. Un messaggio può essere qualsiasi cosa. 
     * Un filtro è un processo, un thread o un altro componente che legge perpetuamente 
     * i messaggi da una pipe di input, uno alla volta, elabora ogni messaggio, quindi 
     * scrive il risultato su una pipe di output. Pertanto, è possibile formare collegamenti(pipes) 
     * di filtri collegati da pipe appunto:
     * 
     *      // Figura 1
     *      
     * L'ispirazione per le architetture di pipeline proviene probabilmente dall'elaborazione 
     * del segnale. In questo contesto un Pipe è un canale di comunicazione che trasporta un 
     * segnale (messaggio) e i filtri sono componenti di elaborazione del segnale come 
     * amplificatori, filtri di rumore, ricevitori e trasmettitori. 
     * Le architetture delle pipeline vengono visualizzate in molti contesti software. 
     * (Appaiono anche in contesti hardware. Ad esempio, molti processori utilizzano architetture 
     * di pipeline.) Gli utenti della shell dei comandi UNIX e DOS creano pipeline collegando 
     * l'output standard di un programma (cioè cout) all'input standard di un altro (ad esempio, cin):
     * 
     *      % cat inFile | | del modello grep sort > outFile
     *      
     *  o in Windows
     *  
     *      dir c: | | echo err 
     *      
     * In questo caso le pipe (ad esempio, "|") sono canali di comunicazione tra processi 
     * forniti dal sistema operativo e i filtri sono tutti i programmi che leggono i messaggi 
     * dall'input standard e ne scrivono i risultati nell'output standard.
     * 
     * I programmatori LISP possono rappresentare le pipe per elenchi e i filtri per procedure 
     * di elaborazione delle liste. Le pipeline sono costruite utilizzando la composizione 
     * procedurale. Si supponga, ad esempio, che siano definite le seguenti procedure LISP. 
     * In ogni caso il parametro nums rappresenta un elenco di numeri interi:
     * 
     * = lista ottenuta rimuovendo i numeri pari dai numeri
     * (define (filterEvens nums) ... )
     * 
     * = lista ottenuta quadrando ogni n in num 
     * (define (mapSquare nums) ... )
     * 
     * = somma di tutti gli n in numeri
     * (define (sum nums) ... )
     * 
     * Possiamo usare queste procedure per costruire una pipeline che somma i quadrati degli 
     * interi dispari:
     *      // Figura2
     *      
     * Ecco la definizione LISP corrispondente:
     * = sum of square di n dispari
     * in numeri
     * (define (sumOddSquares nums)
     * (sum (mapSquare (filterEvens nums))))
     * 
     * Le pipeline sono state utilizzate anche per implementare i compilatori. 
     * Ogni fase della compilazione è un filtro:
     *      // Figura 3
     *      
     * Lo scanner legge un flusso di caratteri da un file di codice sorgente e produce un flusso 
     * di token. Un parser legge un flusso di token e produce un flusso di alberi di analisi. 
     * Un traduttore legge un flusso di alberi di analisi e produce un flusso di istruzioni 
     * in linguaggio assembly. Possiamo inserire nuovi filtri nella pipeline come ottimizzatori 
     * e type checker, oppure possiamo sostituire i filtri esistenti con versioni migliorate.
     * 
     * C'è anche un modello di progettazione della pipeline:
     *   Pipe e Filter 
     * Altri nomi
     *   Condutture
     * Problema
     *   Le fasi di un sistema che elabora flussi di dati devono essere riutilizzabili, 
     *   ordinabili, sostituibili e/o sviluppate in modo indipendente.
     * Soluzione
     *   Implementare il sistema come pipeline. I passaggi vengono implementati come 
     *   oggetti chiamati filtri. I filtri ricevono input e scrivono output su flussi 
     *   chiamati pipe. Un filtro conosce l'identità dei suoi pipe di ingresso e uscita, 
     *   ma non dei filtri vicini.
     *   
     * Classificazione dei filtri
     * 
     *  Esistono quattro tipi di filtri: 
     *          produttori, consumatori, trasformatori e tester. 
     *  
     *  Un produttore è un produttore di messaggi. Non ha un Pipe di ingresso. 
     *  Genera un messaggio nella sua pipe di output. 
     *  
     *  Un consumatore è un consumatore di messaggi. Non ha pipe di uscita. 
     *  Consuma i messaggi presi dal suo pipe di input. 
     *  
     *  Un trasformatore legge un messaggio dal suo tubo di ingresso, lo modula, 
     *  quindi scrive il risultato sul suo pipe di uscita. 
     *  (Questo è ciò che i programmatori DOS e UNIX chiamano filtri.) 
     *  
     *  Un tester legge un messaggio dalla sua pipe di input, quindi lo testa. 
     *  Se il messaggio supera il test, viene scritto, inalterato, sul pipe di output; 
     *  in caso contrario, viene scartato. (Questo è ciò che gli ingegneri di elaborazione 
     *  del segnale chiamano filtri).
     *  
     *  Un filtro attivo ha una funzione di loop di controllo. Ecco una versione semplificata 
     *  che presuppone che il filtro sia un trasformatore:
     *  
     *  void controlLoop()
     *  {
     *      while(true){
     *          Message val = inPipe.read();
     *          val = transform(val); fare qualcosa per val
     *          outPipe.write(val);
     *     }
     *  }
     *  
     *  Quando attivato, un filtro passivo legge un singolo messaggio dalla sua pipe di input, 
     *  lo elabora, quindi scrive il risultato nella sua pipe di output:
     *  
     *  void activate()
     *  {
     *      Message val = inPipe.read();
     *      val = transform(val); // fare qualcosa per val
     *      outPipe.write(val);
     *  }
     *  
     *  Esistono due tipi di filtri passivi. 
     *      
     *      Un filtro basato sui dati viene attivato quando un altro filtro scrive un 
     *      messaggio nella relativa pipe di input. 
     *      
     *      Un filtro basato sulla domanda viene attivato quando un altro filtro tenta 
     *      di leggere un messaggio dalla sua pipe di output vuota.
     *      
     *  Struttura dinamica: data-driven
     *  
     *      Supponiamo che una particolare pipeline basata sui dati sia costituita da 
     *      un produttore collegato a un trasformatore, connesso a un consumatore. 
     *      - Il produttore scrive un messaggio alla pipe 1, il trasformatore legge il messaggio, 
     *      lo trasforma, quindi lo scrive sul pipe 2. 
     *      - Il consumatore legge il messaggio, quindi lo consuma:
     *      
     *      // Figura 4
     *      
     *  Struttura dinamica: Demand-Driven
     *  
     *  Una pipeline basata sui dati invia i messaggi attraverso la pipeline. 
     *  Una pipeline basata sulla domanda estrae i messaggi attraverso la pipeline. 
     *  Immagina la stessa configurazione utilizzando filtri passivi basati sulla domanda. 
     *  Questa volta le operazioni di lettura si propagano dal consumatore al produttore. 
     *  Un messaggio viene prodotto e scritto sulla pipe 1. 
     *  Il trasformatore legge il messaggio, lo trasforma, quindi lo scrive nella pipe 2. 
     *  Questo messaggio è il valore restituito dalla chiamata originale del consumatore 
     *  a read():
     *  
     *      // Figura 5
     *      
     *  Un problema
     *  
     *  Entrambi i diagrammi rivelano un problema di progettazione. Come fa il trasformatore 
     *  a sapere quando chiamare pipe1.read()? In che modo il consumatore basato sui dati 
     *  sa quando chiamare pipe2.read()? In che modo il produttore guidato dalla domanda 
     *  sa quando produrre un messaggio? I filtri attivi risolvono questo problema eseguendo 
     *  il polling delle pipe di ingresso o bloccando quando leggono da una pipes di input vuota, 
     *  ma ciò è possibile solo se ogni filtro è in esecuzione nel proprio thread o processo.
     *  
     *  Potremmo avere il produttore nel modello basato sui dati che segnala il trasformatore 
     *  dopo aver scritto un messaggio nella pipe 1. Il trasformatore potrebbe quindi segnalare 
     *  il consumatore dopo aver scritto un messaggio nella pipe 2. Nel modello basato sulla 
     *  domanda il consumatore potrebbe segnalare il trasformatore quando ha bisogno di dati 
     *  e il trasformatore potrebbe segnalare al produttore quando ha bisogno di dati. 
     *  Ma questa soluzione crea dipendenze tra filtri vicini. Lo stesso trasformatore 
     *  non poteva essere utilizzato in una pipeline diversa con vicini diversi.
     *  Il problema di progettazione si adatta allo stesso schema del problema di come il 
     *  reattore (Modello Rewactor) comunica con i suoi monitor sconosciuti. 
     *  Abbiamo risolto questo problema rendendo il reacot un publicher ai subscriber che monitorano. 
     *  Possiamo usare il modello publicher-subscriber anche qui. 
     *  Le pipe sono publicher e i filtri sono subscripbers. 
     *  Nel modello basato sui dati i filtri sottoscrivono le relative pipe di input. 
     *  Nel modello basato sulla domanda i filtri si sottoscrivono ai rispettivi tubi di output.
     *  
     *  I filtri possono anche essere classificati come attivi o passivi. 
     *      - Un filtro attivo dispone di un ciclo di controllo che viene eseguito nel 
     *      proprio processo o thread. Legge continuamente i messaggi dalla sua pipe di input, 
     *      li elabora, quindi scrive i risultati nella sua pipe di output. 
     *      - Un filtro attivo deve essere derivato da una classe di thread fornita 
     *      dal sistema operativo:
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
