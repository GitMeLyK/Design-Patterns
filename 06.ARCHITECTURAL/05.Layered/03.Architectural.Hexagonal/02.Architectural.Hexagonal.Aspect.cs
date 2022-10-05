using System;

namespace DotNetDesignPatternDemos.Architectural.Hexagonal.Aspect
{

    /*
     *  L'architettura esagonale onsente a un'applicazione di essere ugualmente guidata da utenti, 
     *  programmi, test automatizzati o script batch e di essere sviluppata e testata in isolamento 
     *  dai suoi eventuali dispositivi e database di runtime.
     *  
     *  Quando un driver desidera utilizzare l'applicazione in una porta, invia una richiesta 
     *  che viene convertita da un adattatore per la tecnologia specifica del driver in una 
     *  chiamata o un messaggio di procedura utilizzabile, che la passa alla porta 
     *  dell'applicazione. L'applicazione è beatamente ignorante della tecnologia del conducente. 
     *  
     *  Quando l'applicazione ha qualcosa da inviare, lo invia attraverso una porta a un 
     *  adattatore, che crea i segnali appropriati necessari alla tecnologia ricevente 
     *  (umana o automatizzata). L'applicazione ha un'interazione semanticamente valida con 
     *  gli adattatori su tutti i lati di essa, senza conoscere effettivamente la natura delle 
     *  cose dall'altra parte degli adattatori.
     *  
     *      // Figura Hexagonal-architecture-basic-1.gif
     *      
     *  Motivazione
     *  
     *  Uno dei grandi bugaboo delle applicazioni software nel corso degli anni è stata 
     *  l'infiltrazione della logica di business nel codice dell'interfaccia utente. 
     *  
     *  Il problema che questo causa è triplice:
     *  
     *      - In primo luogo, il sistema non può essere testato in modo ordinato con suite 
     *      di test automatizzate perché parte della logica che deve essere testata dipende 
     *      da dettagli visivi che cambiano spesso come le dimensioni del campo e il 
     *      posizionamento dei pulsanti;
     *      
     *      - Per lo stesso identico motivo, diventa impossibile passare da un uso guidato 
     *      dall'uomo del sistema a un sistema eseguito in batch;
     *      
     *      - Per lo stesso motivo, diventa difficile o impossibile consentire al programma 
     *      di essere guidato da un altro programma quando questo diventa attraente.
     * 
     * La soluzione tentata, ripetuta in molte organizzazioni, è quella di creare un nuovo livello 
     * nell'architettura, con la promessa che questa volta, davvero e veramente, nessuna logica 
     * di business verrà inserita nel nuovo livello. 
     * 
     * Tuttavia, non avendo alcun meccanismo per rilevare quando si verifica una violazione di 
     * tale promessa, l'organizzazione scopre alcuni anni dopo che il nuovo livello è ingombro 
     * di logica di business e il vecchio problema è riapparso.
     * 
     * Immagina ora che "ogni"" funzionalità offerta dall'applicazione fosse disponibile tramite 
     * un'API (interfaccia programmata dell'applicazione) o una chiamata di funzione. 
     * In questo caso, il reparto test o QA può eseguire script di test automatizzati 
     * sull'applicazione per rilevare quando una nuova codifica interrompe una funzione 
     * precedentemente funzionante. Gli esperti aziendali possono creare casi di test 
     * automatizzati, prima che i dettagli della GUI siano finalizzati, che dicono ai 
     * programmatori quando hanno svolto correttamente il loro lavoro (e questi test diventano 
     * quelli eseguiti dal reparto test). 
     * 
     * L'applicazione può essere distribuita in modalità "headless", quindi è disponibile solo 
     * l'API e altri programmi possono utilizzare le sue funzionalità: questo semplifica la 
     * progettazione complessiva di suite di applicazioni complesse e consente inoltre alle 
     * applicazioni di servizi business-to-business di utilizzare l'un l'altro senza l'intervento 
     * umano sul Web. Infine, i test di regressione delle funzioni automatizzati rilevano qualsiasi 
     * violazione della promessa di mantenere la logica di business fuori dal livello di presentazione. 
     * 
     * L'organizzazione è in grado di rilevare, e quindi correggere, la perdita logica.
     * 
     * Un interessante problema simile esiste su quello che normalmente è considerato 
     * "l'altro lato" dell'applicazione, in cui la logica dell'applicazione viene legata a un 
     * database esterno o ad un altro servizio. 
     * 
     * Quando il server di database si arresta o subisce una rielaborazione o una sostituzione 
     * significativa, i programmatori non possono lavorare perché il loro lavoro è legato alla 
     * presenza del database. Ciò causa costi di ritardo e spesso cattivi sentimenti tra le persone.
     * 
     * Non è ovvio che i due problemi siano correlati, ma c'è una simmetria tra loro che si 
     * manifesta nella natura della soluzione.
     * 
     * Natura della soluzione
     * 
     * Sia i problemi lato utente che quelli lato server sono in realtà causati dallo stesso 
     * errore nella progettazione e nella programmazione: l'entanglement tra la logica di 
     * business e l'interazione con entità esterne. L'asimmetria da sfruttare non è quella 
     * tra i lati ''sinistro'' e ''destro'' dell'applicazione, ma tra ''dentro'' e ''fuori'' 
     * dell'applicazione. 
     * La regola a cui obbedire è che il codice relativo alla parte "all'interno" non deve 
     * fuoriuscire nella parte "esterna".
     * 
     * Rimuovendo per un attimo qualsiasi asimmetria sinistra-destra o su-giù, vediamo che 
     * l'applicazione comunica su ''porte'' ad agenzie esterne. La parola "porta" dovrebbe 
     * evocare pensieri di "porte" in un sistema operativo, in cui qualsiasi dispositivo 
     * che aderisce ai protocolli di una porta può essere collegato ad esso; e ''porte'' 
     * sui gadget elettronici, dove ancora una volta, qualsiasi dispositivo che si adatta 
     * ai protocolli meccanici ed elettrici può essere collegato.
     * 
     * Il protocollo per una porta è dato dallo scopo della conversazione tra i due dispositivi.
     * Il protocollo assume la forma di un'api (Application Program Interface).
     * 
     * Per ogni dispositivo esterno c'è un ''adattatore'' che converte la definizione API nei 
     * segnali necessari a quel dispositivo e viceversa. Un'interfaccia utente grafica o GUI è 
     * un esempio di adattatore che mappa i movimenti di una persona all'API della porta. 
     * Altri adattatori che si adattano alla stessa porta sono test harness automatizzati come 
     * FIT o Fitnesse, driver batch e qualsiasi codice necessario per la comunicazione tra 
     * applicazioni in tutta l'azienda o in rete.
     * 
     * Su un altro lato dell'applicazione, l'applicazione comunica con un'entità esterna per 
     * ottenere dati. Il protocollo è in genere un protocollo di database. Dal punto di vista 
     * dell'applicazione, se il database viene spostato da un database SQL a un file flat o a 
     * qualsiasi altro tipo di database, la conversazione attraverso l'API non dovrebbe cambiare. 
     * 
     * Gli adattatori aggiuntivi per la stessa porta includono quindi un adattatore SQL, 
     * un adattatore di file flat e, soprattutto, un adattatore a un database "fittizio", 
     * uno che si trova in memoria e non dipende affatto dalla presenza del database reale.
     * 
     * Molte applicazioni dispongono solo di due porte: la finestra di dialogo lato utente e la 
     * finestra di dialogo lato database. Ciò conferisce loro un aspetto asimmetrico, che rende 
     * naturale creare l'applicazione in un'architettura impilata a unidimensionale, a tre, 
     * quattro o cinque strati.
     * 
     * Ci sono due problemi con questi disegni. Prima e peggio, le persone tendono a non prendere 
     * sul serio le "linee" nel disegno a strati. Lasciano che la logica dell'applicazione 
     * trasmetta attraverso i confini del livello, causando i problemi sopra menzionati. 
     * In secondo luogo, potrebbero esserci più di due porte per l'applicazione, in modo 
     * che l'architettura non si adatti al disegno del livello unidimensionale.
     * 
     * L'architettura esagonale, o porte e adattatori, risolve questi problemi notando la 
     * simmetria nella situazione: c'è un'applicazione all'interno che comunica su un certo 
     * numero di porte con le cose all'esterno. 
     * 
     * Gli elementi al di fuori dell'applicazione possono essere trattati simmetricamente.
     * 
     * L'esagono ha lo scopo di evidenziare visivamente
     * 
     *      (a) l'asimmetria interno-esterno e la natura simile delle porte, per allontanarsi 
     *      dall'immagine stratificata unidimensionale e da tutto ciò che evoca, e 
     *      (b) la presenza di un numero definito di porte diverse – due, tre o quattro 
     *      (quattro è la maggior parte che ho incontrato fino ad oggi).
     *      
     * L'esagono non è un esagono perché il numero sei è importante, ma piuttosto per consentire 
     * alle persone che fanno il disegno di avere spazio per inserire porte e adattatori di cui 
     * hanno bisogno, non essendo vincolati da un disegno a strati unidimensionale. 
     * 
     * Il termine ''architettura esagonale'' deriva da questo effetto visivo.
     * 
     * Il termine "porta e adattatori" riprende gli "scopi" delle parti del disegno. 
     * 
     * Una porta identifica una conversazione mirata. In genere sono presenti più adattatori per 
     * una porta, per varie tecnologie che possono collegarsi a tale porta. In genere, questi 
     * potrebbero includere una segreteria telefonica, una voce umana, un telefono touch-tone, 
     * un'interfaccia umana grafica, un test harness, un driver batch, un'interfaccia http, 
     * un'interfaccia diretta da programma a programma, un database fittizio (in memoria), 
     * un database reale (forse diversi database per lo sviluppo, il test e l'uso reale).
     * 
     * Nelle note applicative, l'asimmetria sinistra-destra verrà nuovamente visualizzata. 
     * Tuttavia, lo scopo principale di questo modello è quello di concentrarsi sull'asimmetria 
     * interno-esterno, fingendo brevemente che tutti gli elementi esterni siano identici dal 
     * punto di vista dell'applicazione.
     * 
     * Struttura
     * 
     *      // Figura 2 : Hexagonal-architecture-with-adapters.gif
     *      
     *  Nella Figura 2 viene illustrata un'applicazione con due porte attive e più schede per 
     *  ogni porta. Le due porte sono il lato di controllo dell'applicazione e il lato di recupero 
     *  dei dati. 
     *  
     *  Questo disegno mostra che l'applicazione può essere guidata in egual misura da una suite 
     *  di test di regressione automatizzata a livello di sistema, da un utente umano, da 
     *  un'applicazione http remota o da un'altra applicazione locale. 
     *  
     *  Sul lato dati, l'applicazione può essere configurata per l'esecuzione disaccoppiata da 
     *  database esterni utilizzando un oracolo in memoria, o ''mock'', sostituzione del database; 
     *  oppure può essere eseguito sul database di test o di runtime. 
     *  
     *  La specifica funzionale dell'applicazione, forse nei casi d'uso, è fatta contro 
     *  l'interfaccia dell'esagono interno e non contro nessuna delle tecnologie esterne che 
     *  potrebbero essere utilizzate.
     *  
     *      // Figura 3 : Hexagonal-architecture-barn-door-image-1.gif
     *      
     *  Nella Figura 3 viene illustrata la stessa applicazione mappata a un disegno architettonico 
     *  a tre livelli. Per semplificare il disegno vengono visualizzati solo due adattatori per 
     *  ogni porta. Questo disegno ha lo scopo di mostrare come più adattatori si adattano ai 
     *  livelli superiore e inferiore e la sequenza in cui i vari adattatori vengono utilizzati 
     *  durante lo sviluppo del sistema. Le frecce numerate mostrano l'ordine in cui un team può 
     *  sviluppare e utilizzare l'applicazione:
     *  
     *       Con un test harness FIT che guida l'applicazione e utilizza il database fittizio 
     *       (in memoria) che sostituisce il database reale;
     *       
     *       Aggiunta di una GUI all'applicazione, ancora in esecuzione dal database fittizio;
     *       
     *       Nei test di integrazione, con script di test automatizzati (ad esempio, dal Cruise 
     *       Control) che guidano l'applicazione contro un database reale contenente dati di test;
     *       
     *       In uso reale, con una persona che utilizza l'applicazione per accedere a un database 
     *       live.
     *       
     *  Codice di esempio
     *  
     *  L'applicazione più semplice che dimostra le porte e gli adattatori fortunatamente viene 
     *  fornita con la documentazione FIT. Si tratta di una semplice applicazione di discount 
     *  computing:
     *  
     *      discount(amount) = amount * rate(amount);
     *      
     *  Nel nostro adattamento, l'importo verrà dall'utente e la tariffa proverrà da un database, 
     *  quindi ci saranno due porte. 
     *  
     *  Li implementiamo in più fasi:
     *  
     *          Con test ma con una velocità costante invece di un database fittizio,
     *          poi con la GUI, quindi con un database fittizio che può essere scambiato 
     *          per un database reale.
     *          
     *  Fase 1: FIT App constant-as-mock-database
     *  
     *      Per prima cosa creiamo i test case come tabella HTML (vedere la documentazione FIT 
     *      per questo):
     *
     *          Metodo TestDiscounter	
     *          importo	sconto()
     *          100	5
     *          200	10
     * Si noti che i nomi delle colonne diventeranno nomi di classi e funzioni nel nostro programma.
     * FIT contiene modi per sbarazzarsi di questo "programmatore", ma per questo articolo è più 
     * facile lasciarli dentro.
     * 
     * Sapendo quali saranno i dati di test, creiamo l'adattatore lato utente, il ColumnFixture 
     * fornito con FIT come spedito:
     * 
     * 
     *  import fit.ColumnFixture; 
     *  public class TestDiscounter extends ColumnFixture 
     *  { 
     *     private Discounter app = new Discounter(); 
     *     public double amount;
     *     public double discount() 
     *     { return app.discount(amount); } 
     *  }
     *  
     *  Questo è in realtà tutto ciò che c'è nell'adattatore. Finora, i test vengono eseguiti 
     *  dalla riga di comando (vedere il libro FIT per il percorso necessario). 
     *  
     *  Abbiamo usato questo:
     *  
     *  set FIT_HOME=/FIT/FitLibraryForFit15Feb2005
     *  java -cp %FIT_HOME%/lib/javaFit1.1b.jar;%FIT_HOME%/dist/fitLibraryForFit.jar;src;bin
     *  fit.FileRunner test/Discounter.html TestDiscount_Output.html
     *  
     *  FIT produce un file di output con colori che ci mostrano cosa è passato (o non è riuscito, 
     *  nel caso in cui abbiamo fatto un errore di battitura da qualche parte lungo la strada).
     *  
     *  A questo punto il codice è pronto per il check-in, l'aggancio al Cruise Control o alla 
     *  tua macchina di compilazione automatizzata e includerlo nella suite build-and-test.
     *  
     *  Fase 2: UI App constant-as-mock-database
     *  
     *  Ti lascerò creare la tua interfaccia utente e farla guidare l'applicazione Discounter, 
     *  poiché il codice è un po 'lungo da includere qui. Alcune delle righe chiave nel codice 
     *  sono queste:
     *
     *  Discounter app = new Discounter();
     * 
     *  public void actionPerformed(ActionEvent event) 
     *  {
     *      ...
     *     String amountStr = text1.getText();
     *     double amount = Double.parseDouble(amountStr);
     *     discount = app.discount(amount));
     *     text3.setText( "" + discount );
     *     ...
     *  }   
     *  
     *  A questo punto l'applicazione può essere sia dimostrata che testata la regressione. 
     *  Gli adattatori lato utente sono entrambi in esecuzione.
     *  
     *  Fase 3: (FIT o UI) Database fittizio dell'app
     *  
     *  Per creare un adattatore sostituibile per il lato database, creiamo un'interfaccia 
     *  per un repository, un ''RepositoryFactory'' che produrrà il database fittizio o 
     *  l'oggetto servizio reale e la simulazione in memoria per il database.
     *  
     *  public interface RateRepository 
     *  {
     *     double getRate(double amount);
     *   }
     *  public class RepositoryFactory 
     *  {
     *     public RepositoryFactory() {  super(); }
     *     public static RateRepository getMockRateRepository() 
     *     {
     *        return new MockRateRepository();
     *     }
     *  }
     *  public class MockRateRepository implements RateRepository 
     *  {
     *     public double getRate(double amount) 
     *     {
     *        if(amount <= 100) return 0.01;
     *        if(amount <= 1000) return 0.02;
     *        return 0.05;
     *      }
     *   }
     *   
     * Per agganciare questo adattatore all'applicazione Discounter, è necessario aggiornare 
     * l'applicazione stessa per accettare un adattatore repository da utilizzare e fare in 
     * modo che l'adattatore lato utente (FIT o UI) passi il repository da utilizzare (reale 
     * o fittizio) nel costruttore dell'applicazione stessa. Ecco l'applicazione aggiornata e 
     * un adattatore FIT che passa in un repository fittizio (il codice dell'adattatore FIT 
     * per scegliere se passare nell'adattatore del repository fittizio o reale è più lungo 
     * senza aggiungere molte nuove informazioni, quindi ometto quella versione qui).
     * 
     *  import repository.RepositoryFactory;
     *  import repository.RateRepository;
     *  public class Discounter 
     *  {
     *     private RateRepository rateRepository;
     *     public Discounter(RateRepository r) 
     *     {
     *        super();
     *        rateRepository = r;
     *      }
     *     public double discount(double amount) 
     *     {
     *        double rate = rateRepository.getRate( amount ); 
     *        return amount * rate;
     *      }
     *  }
     *  import app.Discounter;
     *  import fit.ColumnFixture;
     *  public class TestDiscounter extends ColumnFixture 
     *  {
     *     private Discounter app = 
     *         new Discounter(RepositoryFactory.getMockRateRepository());
     *     public double amount;
     *     public double discount() 
     *     {
     *        return app.discount( amount );
     *     }
     *  }
     *  
     *  Ciò conclude l'implementazione della versione più semplice dell'architettura esagonale.
     *  
     *  Per un'implementazione diversa, utilizzando Ruby e Rack per l'utilizzo del browser, 
     *  vedere https://github.com/totheralistair/SmallerWebHexagon
     *  
     *  Note applicative
     *  
     *  L'asimmetria sinistra-destra
     *  Il modello di porte e adattatori è deliberatamente scritto fingendo che tutte le porte 
     *  siano fondamentalmente simili. Questa pretesa è utile a livello architettonico. 
     *  Nell'implementazione, le porte e gli adattatori si presentano in due versioni, 
     *  che chiamerò "primario" e "secondario", per ragioni presto ovvie. 
     *  Potrebbero anche essere chiamati adattatori ''driving'' e adattatori ''driven''.
     *  
     *  Il lettore di avvisi avrà notato che in tutti gli esempi forniti, gli apparecchi 
     *  FIT sono utilizzati sulle porte sul lato sinistro e i mock sulla destra. 
     *  Nell'architettura a tre strati, FIT si trova nello strato superiore e il finto si 
     *  trova nello strato inferiore.
     *  
     *  Questo è legato all'idea da casi d'uso di "attori primari" e "attori secondari". 
     *  Un ''attore primario'' è un attore che guida l'applicazione (la porta fuori dallo 
     *  stato quiescente per eseguire una delle sue funzioni pubblicizzate). 
     *  
     *  Un "attore secondario" è quello che l'applicazione guida, sia per ottenere risposte 
     *  da o semplicemente per notificare. La distinzione tra ''primario''' e ''secondario'' 
     *  sta in chi innesca o è responsabile della conversazione.
     *  
     *  L'adattatore di test naturale per sostituire un attore "primario" è FIT, poiché tale 
     *  framework è progettato per leggere uno script e guidare l'applicazione. 
     *  L'adattatore di test naturale per sostituire un attore "secondario" come un database 
     *  è una simulazione, poiché è progettato per rispondere a query o registrare eventi 
     *  dall'applicazione.
     *  
     *  Queste osservazioni ci portano a seguire il diagramma del contesto del caso d'uso 
     *  del sistema e a disegnare le "porte primarie" e gli adattatori primari sul lato 
     *  sinistro (o superiore) dell'esagono e le "porte secondarie" e gli "adattatori secondari" 
     *  sul lato destro (o inferiore) dell'esagono.
     *  
     *  La relazione tra porte/adattatori primari e secondari e la loro rispettiva implementazione 
     *  in FIT e mocks è utile da tenere a mente, ma dovrebbe essere utilizzata come conseguenza 
     *  dell'utilizzo dell'architettura delle porte e degli adattatori, non per cortocircuitarla. 
     *  
     *  Il vantaggio finale di un'implementazione di porte e adattatori è la possibilità di eseguire 
     *  l'applicazione in una modalità completamente isolata.
     *  
     *  Casi d'uso e limite dell'applicazione
     *  
     *  È utile utilizzare il modello di architettura esagonale per rafforzare il modo preferito 
     *  di scrivere casi d'uso.
     *  
     *  Un errore comune è quello di scrivere casi d'uso per contenere una conoscenza intima 
     *  della tecnologia che si trova all'esterno di ogni porta. Questi casi d'uso si sono 
     *  guadagnati una reputazione giustamente cattiva nel settore per essere lunghi, difficili 
     *  da leggere, noiosi, fragili e costosi da mantenere.
     *  
     *  Comprendendo l'architettura delle porte e degli adattatori, possiamo vedere che i casi 
     *  d'uso dovrebbero generalmente essere scritti al limite dell'applicazione (l'esagono interno), 
     *  per specificare le funzioni e gli eventi supportati dall'applicazione, indipendentemente 
     *  dalla tecnologia esterna. Questi casi d'uso sono più brevi, più facili da leggere, meno 
     *  costosi da mantenere e più stabili nel tempo.
     *  
     *  Quante porte?
     *  
     *  Che cosa sia e cosa non sia esattamente un porto è in gran parte una questione di gusti. 
     *  Ad un estremo, a ogni caso d'uso potrebbe essere data la propria porta, producendo 
     *  centinaia di porte per molte applicazioni. 
     *  
     *  In alternativa, si potrebbe immaginare di unire tutte le porte primarie e tutte le porte 
     *  secondarie in modo che ci siano solo due porte, un lato sinistro e un lato destro.
     *  
     *  Nessuno dei due estremi sembra ottimale.
     *  
     *  Il sistema meteorologico descritto negli Usi noti ha quattro porte naturali: 
     *  il feed meteo, l'amministratore, gli abbonati notificati, il database degli abbonati. 
     *  Un controller per macchine da caffè ha quattro porte naturali: l'utente, il database 
     *  contenente le ricette e i prezzi, i distributori e la scatola delle monete. 
     *  
     *  Un sistema di farmaci ospedalieri potrebbe averne tre: uno per l'infermiere, uno per 
     *  il database delle prescrizioni e uno per i distributori di farmaci per il controllo 
     *  del computer.
     *  
     *  Non sembra che ci sia alcun danno particolare nella scelta del numero "sbagliato" 
     *  di porte, quindi rimane una questione di intuizione. La mia selezione tende a favorire 
     *  un piccolo numero, due, tre o quattro porte, come descritto sopra e negli Usi noti.
     *  
     *      // Figura Hexagonal-architecture-complex-example.gif
     *      
     *  Nella Figura 4 viene illustrata un'applicazione con quattro porte e più schede in 
     *  corrispondenza di ogni porta. Ciò è derivato da un'applicazione che ascoltava gli 
     *  avvisi del servizio meteorologico nazionale su terremoti, tornado, incendi e 
     *  inondazioni e informava le persone sui loro telefoni o segreterie telefoniche. 
     *  
     *  Nel momento in cui abbiamo discusso di questo sistema, le interfacce del sistema 
     *  sono state identificate e discusse dalla "tecnologia, legata allo scopo". 
     *  
     *  C'era un'interfaccia per i dati trigger che arrivavano tramite un feed di fili, una 
     *  per i dati di notifica da inviare alle segreterie telefoniche, un'interfaccia 
     *  amministrativa implementata in una GUI e un'interfaccia di database per ottenere 
     *  i dati degli abbonati.
     *  
     *  Le persone stavano lottando perché avevano bisogno di aggiungere un'interfaccia 
     *  http dal servizio meteo, un'interfaccia e-mail ai loro abbonati, e dovevano trovare 
     *  un modo per raggruppare e separare la loro crescente suite di applicazioni per le 
     *  diverse preferenze di acquisto dei clienti. Temevano di essere alla ricerca di un 
     *  incubo di manutenzione e test mentre dovevano implementare, testare e mantenere 
     *  versioni separate per tutte le combinazioni e le permutazioni.
     *  
     *  Il loro cambiamento nella progettazione è stato quello di progettare le interfacce 
     *  del sistema "per scopo" piuttosto che per tecnologia, e di avere le tecnologie 
     *  sostituibili (su tutti i lati) da adattatori. Hanno immediatamente raccolto la 
     *  possibilità di includere il feed http e la notifica e-mail (i nuovi adattatori 
     *  sono mostrati nel disegno con linee tratteggiate). 
     *  
     *  Rendendo ogni applicazione eseguibile in modalità headless tramite API, è possibile 
     *  aggiungere un adattatore app-to-add e disaggregare la suite di applicazioni, collegando 
     *  le sottoapplicazioni su richiesta. Infine, rendendo ogni applicazione eseguibile 
     *  completamente isolata, con adattatori di test e simulazione in atto, hanno acquisito 
     *  la capacità di testare la regressione delle loro applicazioni con script di test 
     *  automatizzati autonomi.
     *  
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
