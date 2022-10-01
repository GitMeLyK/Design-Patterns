using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DotNetDesignPatternDemos.Concurrency.BindingProperties
{
    /*
     * Questo modello di progettazione usato per l'associazione delle proprietà di un oggetto e/o
     * componente è ampiamente usato nel framework di .net ed è documentato, qui terremo conto
     * del pattern per antanomasia con il rispettivo esempio c++, ma rifrangente a quello che
     * nel framework .net c# viene usato.
     * 
     * Esistono due tipi di binding. 
     *  - L'associazione unidirezionale deve essere applicata quando una delle proprietà è di sola lettura. 
     *  - In altri casi, deve essere applicata la regola bidirezionale. 
     *  
     *  I loop infiniti possono essere eliminati bloccando il segnale o confrontando il valore 
     *  assegnato con il valore della proprietà prima dell'assegnazione o eliminando le 
     *  assegnazioni non necessarie. 
     *  
     *  Le proprietà di associazione di diversi tipi possono essere ottenute tramite 
     *  le conversioni di tipo. 
     *  
     *  Le proprietà di associazione con trasformazioni possono essere ottenute riducendo 
     *  la funzione di trasformazione al problema delle proprietà di associazione e la 
     *  funzione può essere considerata immaginariamente come conversioni di tipo. 
     * 
     *  Le proprietà vengono mantenute sincronizzate automaticamente. 
     *  Tra le chiamate di libreria hanno sempre i valori espressi da EqualityConstraints
     *  
     *  Come carenza abbiamo che Il meccanismo di controllo delle modifiche alle proprietà acquisisce alcune risorse.
     *  
     *  Esempio di comunicazione Unidirezionale in c++
     *  
     *  bind_multiple_one_way(src_obj, src_prop, dst_objs[], dst_props[])
     *  {
     *      for (i, j) in (dst_objs, dst_props)
     *      {
     *          bind_properties_one_way(src_obj, src_prop, i, j);
     *      }
     *  }
     *  
     *  Esempio di comunicazione Bidirezionale in c++
     *  
     *  on_property_change(src_prop, dst_prop)
     *  {
     *     block_signal(src_obj, on_property_change);
     *     dst_prop := src_prop;
     *     unblock_signal(src_obj, on_property_change);
     *  }
     *  
     *  Questo che abbiamo visto è come le singole proprietà di un oggetto si riflettono nell'insieme
     *  della procedura per aggiornare di fatto il valore dell'attributo proprietà interno dello stato
     *  dell'oggetto.
     *  
     *  In C# questo aspetto nel framework è legato molto al DataBinding che per definizione viene
     *  proiettato come metodi del framewrok per associare i dati a componenti visivi.
     *  
     *  Lo svantaggio dell'implementazione dell'associazione dati è sempre stata la necessità di stringhe 
     *  magiche e codice boilerplate, sia per trasmettere le modifiche alle proprietà che per associare 
     *  gli elementi dell'interfaccia utente ad esse. 
     *  Nel corso degli anni, sono arrivati vari toolkit e tecniche per ridurre il dolore; 
     *  questo stdio mira a semplificare ulteriormente il processo.
     *  
     *  Innanzitutto, esaminerò le basi dell'implementazione dell'associazione dati, nonché 
     *  le tecniche comuni per semplificarlo (se hai già familiarità con l'argomento, 
     *  sentiti libero di saltare quelle sezioni). 
     *  
     *  Successivamente, svilupperò una tecnica che potresti non aver 
     *  considerato ("A Third Way") e introdurrò soluzioni alle relative difficoltà di 
     *  progettazione incontrate durante lo sviluppo di applicazioni utilizzando MVVM. 
     *  
     *  È possibile ottenere la versione finita del framework che sviluppo qui nel download 
     *  del codice di accompagnamento o aggiungere il pacchetto NuGet SolSoft.DataBinding 
     *  ai propri progetti.
     *  
     *  L'implementazione di INotifyPropertyChanged è il modo preferito per consentire 
     *  l'associazione di un oggetto a un'interfaccia utente. 
     *  È abbastanza semplice, contiene un solo membro: l'evento PropertyChanged. 
     *  L'oggetto deve generare questo evento quando una proprietà associabile viene modificata, 
     *  al fine di notificare alla visualizzazione che deve aggiornare la rappresentazione del 
     *  valore della proprietà.
     *  L'interfaccia è semplice, ma implementarla non lo è. L'aumento manuale dell'evento 
     *  con nomi di proprietà testuali hardcoded non è una soluzione scalabile, né regge 
     *  al refactoring: è necessario assicurarsi che il nome testuale rimanga sincronizzato 
     *  con il nome della proprietà nel codice. 
     *  Questo non vi renderà cari ai vostri successori. 
     *      -- Esempio 1
     *  
     *  Tecnica comune 1: Classe base
     *  
     *      Un modo per semplificare la situazione è con una classe base, 
     *      al fine di riutilizzare parte della logica boilerplate. 
     *      In questo modo vengono inoltre forniti alcuni modi per ottenere il 
     *      nome della proprietà a livello di codice, anziché doverlo codificare.
     *      
     *      Ottenere il nome della proprietà con le espressioni: in .NET Framework 3.5 
     *      sono state introdotte espressioni che consentono l'ispezione in fase di 
     *      esecuzione della struttura del codice. 
     *      LINQ utilizza questa API con grande effetto, ad esempio per convertire 
     *      le query LINQ .NET in istruzioni SQL. Anche gli sviluppatori intraprendenti 
     *      hanno sfruttato questa API per ispezionare i nomi delle proprietà. 
     *      Utilizzando una classe base per eseguire questa ispezione, il setter 
     *      precedente potrebbe essere riscritto come:
     *      -- Esempio 2  
     *
     * Tecnica comune 2: programmazione orientata agli aspetti
     * 
     *  La programmazione orientata agli aspetti (AOP) è una tecnica che fondamentalmente 
     *  "post-elabora" il codice compilato, sia in fase di esecuzione che con una fase di 
     *  post-compilazione, al fine di aggiungere determinati comportamenti (noto come "aspetto"). 
     *  Di solito, l'obiettivo è quello di sostituire il codice boilerplate ripetitivo, come 
     *  la registrazione o la gestione delle eccezioni (le cosiddette "preoccupazioni trasversali"). 
     *  Non sorprende che l'implementazione di INotifyPropertyChanged sia un buon candidato.
     *  
     *  Sono disponibili diversi toolkit per questo approccio. PostSharp è uno (bit.ly/1Xmq4n2). 
     *  Sono rimasto piacevolmente sorpreso nell'apprendere che gestisce correttamente le 
     *  proprietà dipendenti (ad esempio, la proprietà FullName descritta in precedenza). 
     *  Un framework open source chiamato "Fody" è simile (bit.ly/1wXR2VA).
     *  
     *  Questo è un approccio attraente; i suoi svantaggi potrebbero non essere significativi. 
     *  Alcune implementazioni intercettano il comportamento in fase di esecuzione, il che 
     *  comporta un costo delle prestazioni. I framework post-compilazione, al contrario, non 
     *  dovrebbero introdurre alcun sovraccarico di runtime, ma potrebbero richiedere una sorta 
     *  di installazione o configurazione. 
     *  PostSharp è attualmente offerto come estensione di Visual Studio. 
     *  La sua edizione gratuita "Express" limita l'uso dell'aspetto INotifyPropertyChanged 
     *  a 10 classi, quindi questo probabilmente significa un costo monetario. 
     *  Fody, d'altra parte, è un pacchetto NuGet gratuito, che lo fa sembrare una scelta 
     *  convincente. Indipendentemente da ciò, considera che con qualsiasi framework AOP il 
     *  codice che scrivi non è esattamente lo stesso codice che eseguirai ... e debug.
     *
     * Una terza via
     * 
     * Un modo alternativo di gestire questo è quello di sfruttare il design orientato agli oggetti: 
     *  - avere le proprietà stesse responsabili dell'aumento degli eventi! Non è un'idea 
     *    particolarmente rivoluzionaria, ma non è un'idea che ho incontrato al di fuori dei 
     *    miei progetti. Nella sua forma più semplice, potrebbe assomigliare al seguente:
     *    
     *    public class NotifyProperty<T>
     *    {
     *      public NotifyProperty(INotifyPropertyChanged owner, string name, T initialValue);
     *      public string Name { get; }
     *      public T Value { get; }
     *      public void SetValue(T newValue);
     *    }
     *    
     *  L'idea è quella di fornire alla proprietà il suo nome e un riferimento al suo proprietario 
     *  e lasciare che faccia il lavoro di generazione dell'evento PropertyChanged, 
     *  qualcosa del tipo:
     *  
     *   public void SetValue(T newValue)
     *   {
     *     if(newValue != m_value)
     *     {
     *       m_value = newValue;
     *       m_owner.PropertyChanged(m_owner, new PropertyChangedEventArgs(Name));
     *     }
     *   }
     *   
     * Il problema è che questo in realtà non funzionerà: 
     *  - non posso sollevare un evento da un'altra classe del genere. 
     *  - Ho bisogno di una sorta di contratto con la classe proprietaria per permettermi di 
     *    aumentare il suo evento PropertyChanged: questo è esattamente il lavoro di 
     *    un'interfaccia, quindi ne creerò uno:
     *  
     *  public interface IRaisePropertyChanged
     *   {
     *     void RaisePropertyChanged(string propertyName)
     *   }
     *   
     *   Una volta che ho questa interfaccia, posso effettivamente implementare 
     *   NotifyProperty.SetValue:
     *   
     *   public void SetValue(T newValue)
     *   {
     *     if(newValue != m_value)
     *     {
     *       m_value = newValue;
     *       m_owner.RaisePropertyChanged(this.Name);
     *     }
     *   }
     *   
     *   L'Implementazione di IRaisePropertyChanged: 
     *    - richiedere al proprietario della proprietà di implementare un'interfaccia significa 
     *      che ogni classe del modello di visualizzazione richiederà un boilerplate, 
     *      come illustrato nella Figura 1. La prima parte è necessaria affinché qualsiasi 
     *      classe implementi INotifyPropertyChanged; 
     *     - la seconda parte è specifica per il nuovo IRaisePropertyChanged. 
     *       Si noti che, poiché il metodo RaisePropertyChanged non è destinato all'uso generale, 
     *       preferisco implementarlo in modo esplicito.
     *       
     *       Esempio 3
     *       
     *   Potrei mettere questo boilerplate in una classe base ed estenderlo, il che sembra 
     *   riportarmi alla mia discussione precedente. Dopotutto, se applico CallerMemberName al 
     *   metodo RaisePropertyChanged, ho praticamente reinventato la prima tecnica, 
     *   quindi qual è il punto? 
     *   In entrambi i casi, potrei semplicemente copiare il boilerplate in altre classi se non 
     *   possono derivare da una classe base.
     *   
     *   Una differenza fondamentale rispetto alla precedente tecnica di classe base è che 
     *   in questo caso non c'è una vera logica nel boilerplate; tutta la logica è incapsulata 
     *   nella classe NotifyProperty. 
     *   Verificare se il valore della proprietà è cambiato prima di generare l'evento è 
     *   una logica semplice, ma è ancora meglio non duplicarlo. 
     *   Considera cosa accadrebbe se volessi utilizzare un IEqualityComparer diverso per 
     *   eseguire il controllo. Con questo modello, è necessario modificare solo 
     *   la classe NotifyProperty. Anche se si dispone di più classi con lo stesso 
     *   boilerplate IRaisePropertyChanged, ogni implementazione potrebbe trarre 
     *   vantaggio dalle modifiche apportate a NotifyProperty senza dover 
     *   modificare il codice stesso. 
     *   Indipendentemente da eventuali modifiche al comportamento che si desidera introdurre, 
     *   è molto improbabile che il codice IRaisePropertyChanged cambi.
     *   
     *   ----
     *   
     *   Con l'approccio preceente , pagherai comunque un costo di riflessione, ma solo quando crei un 
     *   oggetto, piuttosto che ogni volta che una proprietà cambia. 
     *   Se è ancora troppo costoso (si stanno creando molti oggetti), è sempre possibile memorizzare 
     *   nella cache una chiamata a GetName e mantenerla come valore di sola lettura statico nella 
     *   classe del modello di visualizzazione. 
     *   In entrambi i casi, nell'esempio 4 viene illustrato un esempio di modello di visualizzazione 
     *   semplice.
     *      Esempio 4
     *      
     *   È un po 'strano avere quell'oggetto nullo esclusivamente per sfruttare la funzionalità delle 
     *   espressioni, ma funziona (non ne hai bisogno se hai accesso a nameof).
     *      Trovo questa tecnica preziosa, ma riconosco i compromessi. 
     *      Tra i lati positivi, se rinomino la proprietà UserName, posso essere sicuro che il 
     *      refactoring funzionerà. 
     *      Un altro vantaggio significativo è che "Trova tutti i riferimenti" funziona proprio 
     *      come previsto.
     *      
     *      Sul lato negativo, non è necessariamente così semplice e naturale come fare 
     *      l'associazione in XAML e mi impedisce di mantenere il design dell'interfaccia 
     *      utente "indipendente". Non posso semplicemente riprogettare l'aspetto nello 
     *      strumento Blend senza modificare il codice, ad esempio. 
     *      Inoltre, questa tecnica non funziona con i modelli di dati; è possibile estrarre 
     *      il modello in un controllo personalizzato, ma è più impegnativo.
     *      
     *      In totale, ottengo flessibilità per cambiare il lato "modello dati", a scapito 
     *      della flessibilità sul lato "vista". 
     *      Nel complesso, dipende da te se i vantaggi giustificano la dichiarazione 
     *      dei binding in questo modo.
     *      
     *  Proprietà "derivate"
     *  
     *      In precedenza, ho descritto uno scenario in cui è particolarmente scomodo generare 
     *      l'evento PropertyChanged, in particolare per quelle proprietà il cui valore dipende 
     *      da altre proprietà. 
     *      Ho menzionato il semplice esempio di una proprietà FullName che dipende da FirstName 
     *      e LastName. 
     *      Il mio obiettivo per l'implementazione di questo scenario è quello di prendere in 
     *      considerazione quegli oggetti NotifyProperty di base (FirstName e LastName), 
     *      nonché la funzione per calcolare il valore derivato da essi (ad esempio, FirstName.Value 
     *      + " " + LastName.Value) e, con ciò, produrre un oggetto proprietà che gestirà 
     *      automaticamente il resto per me. 
     *      Per abilitare questo, ci sono un paio di modifiche che farò al mio NotifyProperty originale.
     *      
     *      La prima attività consiste nell'esporre un evento ValueChanged separato in NotifyProperty. 
     *      La proprietà derivata ascolterà questo evento sulle proprietà sottostanti e risponderà 
     *      calcolando un nuovo valore (e aumentando l'evento PropertyChanged appropriato per se stessa). 
     *      
     *      La seconda attività consiste nell'estrarre un'interfaccia, IProperty<T>, per incapsulare 
     *      la funzionalità generale di NotifyProperty. 
     *      
     *      Tra le altre cose, questo mi permette di avere proprietà derivate provenienti da altre 
     *      proprietà derivate. 
     *      
     *      L'interfaccia risultante è semplice ed è elencata qui (le modifiche corrispondenti 
     *      a NotifyProperty sono molto semplici, quindi non le elencherò):
     *      
     *      public interface IProperty<TValue>
     *       {
     *         string Name { get; }
     *         event EventHandler<ValueChangedEventArgs> ValueChanged;
     *         TValue Value { get; }
     *       }
     *
     *      Anche la creazione della classe DerivedNotifyProperty sembra semplice, 
     *      finché non si inizia a provare a mettere insieme i pezzi. 
     *      L'idea di base era quella di prendere le proprietà sottostanti e una funzione per 
     *      calcolare un nuovo valore da loro, ma che immediatamente si trova in difficoltà 
     *      a causa dei generici. 
     *      Non esiste un modo pratico per accettare più tipi di proprietà diversi:
     *
     *      // Attempted constructor
     *       public DerivedNotifyProperty(IRaisePropertyChanged owner,
     *         string propertyName, IProperty<T1> property1, IProperty<T2> property2,
     *         Func<T1, T2, TDerived> derivedValueFunction)
     *
     *      Posso aggirare la prima metà del problema (accettando più tipi generici) 
     *      usando invece i metodi Statici Create:
     *      
     *      static DerivedNotifyProperty<TDerived> CreateDerivedNotifyProperty
     *          <T1, T2, TDerived>(this IRaisePropertyChanged owner,
     *              string propertyName, IProperty<T1> property1, IProperty<T2> property2,
     *              Func<T1, T2, TDerived> derivedValueFunction)
     *              
     *      Tuttavia, la proprietà derivata deve ancora essere in ascolto dell'evento ValueChanged 
     *      in ogni proprietà di base. 
     *      Risolvere questo problema richiede due passaggi. 
     *      Innanzitutto, estrarrò l'evento ValueChanged in un'interfaccia separata:
     *      
     *      public interface INotifyValueChanged // No generic type!
     *      {
     *        event EventHandler<ValueChangedEventArgs> ValueChanged;
     *      }
     *      public interface IProperty<TValue> : INotifyValueChanged
     *      {
     *        string Name { get; }
     *        TValue Value { get; }
     *      }
     *      
     *      Ciò consente a DerivedNotifyProperty di accettare INotifyValueChanged 
     *      non generico anziché IProperty<T> generico. In secondo luogo, ho bisogno di calcolare 
     *      il nuovo valore senza generici: prenderò l'originale derivedValueFunction che accetta 
     *      i due parametri generici e da ciò creerò una nuova funzione anonima che non richiede 
     *      alcun parametro, invece, farà riferimento ai valori delle due proprietà passate. 
     *      In altre parole, creerò una chiusura. 
     *      È possibile visualizzare questo processo nel codice seguente:
     *      
     *      static DerivedNotifyProperty<TDerived> CreateDerivedNotifyProperty
     *        <T1, T2, TDerived>(this IRaisePropertyChanged owner,
     *        string propertyName, IProperty<T1> property1, IProperty<T2> property2,
     *        Func<T1, T2, TDerived> derivedValueFunction)
     *          {   
     *            // Closure
     *            Func<TDerived> newDerivedValueFunction =
     *              () => derivedValueFunction (property1.Value, property2.Value);
     *            return new DerivedNotifyProperty<TValue>(owner, propertyName,
     *              newDerivedValueFunction, property1, property2);
     *          }
     *          
     *     La nuova funzione "valore derivato" è solo Func<TDerived> senza parametri; ora 
     *     DerivedNotifyProperty non richiede alcuna conoscenza dei tipi di proprietà sottostanti, 
     *     quindi posso crearne felicemente uno da più proprietà di tipi diversi.
     *     
     *     L'altra sottigliezza è quando chiamare effettivamente quella funzione di valore derivato.
     *     Un'implementazione ovvia sarebbe quella di ascoltare l'evento ValueChanged di ogni 
     *     proprietà sottostante e chiamare la funzione ogni volta che una proprietà cambia, 
     *     ma ciò è inefficiente quando più proprietà sottostanti cambiano nella stessa 
     *     operazione (immagina un pulsante "Reset" che cancella un modulo). 
     *     Un'idea migliore consiste nel produrre il valore su richiesta (e memorizzarlo nella cache) 
     *     e invalidarlo se una delle proprietà sottostanti cambia. Lazy<T> è un modo perfetto 
     *     per implementarlo.
     *     
     *     È possibile visualizzare un elenco abbreviato della classe 
     *     DerivedNotifyProperty nell'esempio 5. 
     *     Si noti che la classe accetta un numero arbitrario di proprietà da ascoltare: 
     *     sebbene si elenchi solo il metodo Create per due proprietà sottostanti, 
     *     si creano overload aggiuntivi che accettano una proprietà sottostante, 
     *     tre proprietà sottostanti e così via.
     *     
     *     Esempio 5
     *     
     *     Si noti che le proprietà sottostanti potrebbero provenire da proprietari diversi. 
     *     Si supponga, ad esempio, di disporre di un modello di visualizzazione Indirizzo 
     *     con una proprietà IsAddressValid. 
     *     È inoltre disponibile un modello di visualizzazione Ordine che contiene due modelli 
     *     di visualizzazione Indirizzo, per gli indirizzi di fatturazione e spedizione. 
     *     Sarebbe ragionevole creare una proprietà IsOrderValid nel modello di visualizzazione 
     *     Order padre che combini le proprietà IsAddressValid dei modelli di visualizzazione 
     *     Address figlio, in modo da poter inviare l'ordine solo se entrambi gli indirizzi sono validi. 
     *     A tale scopo, il modello di visualizzazione Indirizzo esporrebbe sia Bool 
     *     IsAddressValid { get; } che IProperty<bool> IsAddressValidProperty { get; }, in modo che 
     *     il modello di visualizzazione Order possa creare un oggetto DerivedNotifyProperty che fa 
     *     riferimento agli oggetti IsAddressValidProperty figlio.
     *     
     *     L'utilità di DerivedNotifyProperty
     *     
     *     L'esempio Di FullName che ho dato per una proprietà derivata è abbastanza artificioso, 
     *     ma voglio discutere alcuni casi d'uso reali e collegarli ad alcuni principi di 
     *     progettazione. 
     *     Ho appena accennato a un esempio: IsValid. 
     *     Questo è un modo abbastanza semplice e potente per disabilitare il pulsante "Salva" 
     *     su un modulo, ad esempio. Si noti che non vi è nulla che ti costringa a utilizzare 
     *     questa tecnica solo nel contesto di un modello di visualizzazione dell'interfaccia utente. 
     *     È possibile utilizzarlo anche per convalidare gli oggetti business; hanno solo bisogno 
     *     di implementare IRaisePropertyChanged.
     *     
     *     Una seconda situazione in cui le proprietà derivate sono estremamente utili è in 
     *     scenari di "drill-down". Come semplice esempio, si consideri una casella combinata 
     *     per la selezione di un paese, in cui la selezione di un paese popola un elenco di città. 
     *     È possibile fare in modo che SelectedCountry sia un notifyProperty e, dato un metodo 
     *     GetCitiesForCountry, creare AvailableCities come DerivedNotifyProperty che rimarrà 
     *     automaticamente sincronizzato quando il paese selezionato cambia.
     *     
     *     Una terza area in cui sono stati utilizzati gli oggetti NotifyProperty è per indicare 
     *     se un oggetto è "occupato". Mentre l'oggetto è considerato occupato, alcune funzionalità 
     *     dell'interfaccia utente dovrebbero essere disabilitate e forse l'utente dovrebbe vedere 
     *     un indicatore di avanzamento. Questo è uno scenario apparentemente semplice, ma c'è 
     *     molta sottigliezza qui da colpire.
     *     
     *     La prima parte è il monitoraggio se l'oggetto è occupato; nel caso semplice, posso 
     *     farlo con un Boolean NotifyProperty. Tuttavia, ciò che spesso accade è che un oggetto 
     *     può essere "occupato" per uno dei molteplici motivi: diciamo che sto caricando diverse 
     *     aree di dati, possibilmente in parallelo. Lo stato "occupato" complessivo dovrebbe 
     *     dipendere dal fatto che uno di questi elementi sia ancora in corso. 
     *     Sembra quasi un lavoro per le proprietà derivate, ma sarebbe goffo (se non impossibile): 
     *     avrei bisogno di una proprietà per ogni possibile operazione per tenere traccia se è 
     *     in corso. Invece, voglio fare qualcosa di simile al seguente per ogni operazione, 
     *     usando una singola proprietà IsBusy:
     *     
     *     try
     *      {
     *        IsBusy.SetValue(true);
     *        await LongRunningOperation();
     *      }
     *      finally
     *      {
     *        IsBusy.SetValue(false);
     *      }
     *      
     *     Per abilitare questa opzione, creo una classe IsBusyNotifyProperty che estende 
     *     NotifyProperty<bool> e in essa mantiene un "conteggio occupato". Eseguo l'override di 
     *     SetValue in modo tale che SetValue(true) aumenti tale conteggio e SetValue(false) lo diminuisca. 
     *     Quando il conteggio va da 0 a 1, solo allora chiamo base. SetValue(true), e quando va da 1 a 0, 
     *     base. SetValue(false). In questo modo, l'avvio di più operazioni in sospeso fa sì che 
     *     IsBusy diventi vero solo una volta, e successivamente diventa di nuovo falso solo quando 
     *     sono tutte finite. 
     *     
     *     Questo si occupa del lato "occupato" delle cose: posso legare "è occupato" alla 
     *     visibilità di un indicatore di progresso. Tuttavia, per disabilitare l'interfaccia 
     *     utente, ho bisogno del contrario. 
     *     Quando "è occupato" è true, "UI enabled" dovrebbe essere false.
     *     
     *     XAML ha il concetto di IValueConverter, che converte un valore in (o da) una 
     *     rappresentazione di visualizzazione. Un esempio onnipresente è 
     *     BooleanToVisibilityConverter: in XAML, la "Visibilità" di un elemento non è descritta 
     *     da un valore booleano, ma piuttosto da un valore di enumerazione. 
     *     Ciò significa che non è possibile associare la visibilità di un elemento direttamente 
     *     a una proprietà booleana (come IsBusy); è necessario associare il valore e utilizzare 
     *     anche un convertitore. 
     *     Per esempio:
     *     
     *     <StackPanel Visibility="{Binding IsBusy,Converter={StaticResource BooleanToVisibilityConverter}}" />
     *     
     *     Ho detto che "abilita l'interfaccia utente" è l'opposto di "è occupato"; potrebbe essere allettante 
     *     creare un convertitore di valori per invertire una proprietà booleana e utilizzarlo per eseguire 
     *     il lavoro:
     *     
     *     <Grid IsEnabled="{Binding IsBusy, Converter={StaticResource BooleanToInverseConverter}}" />
     *     
     *     In effetti, prima di creare una classe DerivedNotifyProperty, quello era il modo più semplice. Era piuttosto noioso creare una proprietà separata, collegarla per essere l'inverso di IsBusy e generare l'evento PropertyChanged appropriato. Ora, tuttavia, è banale, e senza quella barriera artificiale (cioè la pigrizia) ho un senso migliore di dove ha senso usare IValueConverter.
     *     
     *     In definitiva, la visualizzazione, indipendentemente dal modo in cui potrebbe essere implementata 
     *     (WPF o Windows Form, ad esempio, o anche un'app console è un tipo di visualizzazione), deve 
     *     essere una visualizzazione (o "proiezione") di ciò che accade nell'applicazione sottostante 
     *     e non ha alcuna responsabilità nel determinare il meccanismo e le regole di business per ciò 
     *     che sta accadendo. In questo caso, il fatto che IsBusy e IsEnabled siano correlati tra loro così 
     *     intimamente è un dettaglio di implementazione; non è intrinseco che la disabilitazione 
     *     dell'interfaccia utente debba essere correlata specificamente al fatto che l'applicazione 
     *     sia occupata.
     *     
     *     Allo stato attuale, la considero un'area grigia e non discuterei con te se volessi 
     *     usare un convertitore di valori per implementarlo. Tuttavia, posso fare un caso molto 
     *     più forte aggiungendo un altro pezzo all'esempio. Facciamo finta che se perde l'accesso 
     *     alla rete, l'applicazione dovrebbe anche disabilitare l'interfaccia utente (e mostrare 
     *     un pannello che indica la situazione). Bene, questo rende tre situazioni: se 
     *     l'applicazione è occupata, dovrei disabilitare l'interfaccia utente (e mostrare 
     *     un pannello di avanzamento). Se l'applicazione perde l'accesso alla rete, dovrei 
     *     anche disabilitare l'interfaccia utente (e mostrare un pannello "connessione persa"). 
     *     La terza situazione è quando l'applicazione è connessa e non occupata e, quindi, 
     *     pronta ad accettare input.
     *     
     *     Cercare di implementarlo senza una proprietà IsEnabled separata è imbarazzante nella 
     *     migliore delle ipotesi; è possibile utilizzare un MultiBinding, ma è ancora 
     *     sgraziato e non supportato in tutti gli ambienti. In definitiva, quel tipo di imbarazzo 
     *     di solito significa che c'è un modo migliore, e ora sappiamo che c'è: questa logica 
     *     è meglio gestita all'interno del modello di visualizzazione. Ora è banale esporre 
     *     due NotifyProperties, IsBusy e IsDisconnected, e quindi creare un 
     *     DerivedNotifyProperty, IsEnabled, che è vero solo se entrambi sono falsi.
     *     
     *     Se hai scelto la route IValueConverter e hai associato lo stato Abilitato 
     *     dell'interfaccia utente direttamente a IsBusy (con un convertitore per invertirlo), 
     *     avresti un bel po 'di lavoro da fare ora. Se invece è stata esposta una proprietà 
     *     IsEnabled derivata separata, l'aggiunta di questo nuovo bit di logica è molto meno 
     *     impegnativa e l'associazione IsEnabled stessa non dovrebbe nemmeno essere modificata. 
     *     Questo è un buon segno che stai facendo le cose per bene.
     *     
     *     Conclusione
     *     
     *     Disporre questo framework è stato un po 'un viaggio, ma la ricompensa è che ora 
     *     posso implementare notifiche di modifica della proprietà senza boilerplate ripetitivo, 
     *     senza stringhe magiche e con supporto per il refactoring. I miei modelli di 
     *     visualizzazione non richiedono logica da una particolare classe base. Posso creare 
     *     proprietà derivate che generano anche le notifiche di modifica appropriate senza 
     *     troppi sforzi aggiuntivi. Infine, il codice che vedo è il codice in esecuzione. 
     *     E ottengo tutto questo sviluppando un framework abbastanza semplice con un design 
     *     orientato agli oggetti. 
     *     
     */

    // Esempio 1
    public class DataBindingObject
    {
        private int m_unreadItemCount;

        public int UnreadItemCount
        {
            get
            {
                return m_unreadItemCount;
            }
            set
            {
                m_unreadItemCount = value;
                OnNotifyPropertyChanged(
                    new PropertyChangedEventArgs("UnreadItemCount")
                ); // Yuck
            }
        }
        private void OnNotifyPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
           // fa qualcosa
        }
    }

    // Esempio 2
    public class DataBindingObject2
    {
        private int m_unreadItemCount;

        public int UnreadItemCount
        {
            get
            {
                return m_unreadItemCount;
            }
            set
            {
                m_unreadItemCount = value;
                RaiseNotifyPropertyChanged(() => UnreadItemCount);
            }
        }

        // In questo modo, la ridenominazione di UnreadItemCount rinominerà anche il riferimento
        // all'espressione, in modo che il codice continui a funzionare.
        // La firma di RaiseNotifyPropertyChanged sarebbe la seguente:
        private void RaiseNotifyPropertyChanged<T>(Expression<Func<T>> memberExpression)
        {
            // Fa qualcosa
        }

        // Esistono varie tecniche per recuperare il nome della proprietà da memberExpression.
        // Il blog MSDN di C# 
        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        // Ottenere il nome della proprietà con CallerMemberName: C# 5.0 e .NET Framework 4.5
        // ha portato un modo aggiuntivo per recuperare il nome della proprietà, utilizzando
        // l'attributo CallerMemberName (è possibile utilizzarlo con versioni precedenti
        // di .NET Framework tramite il pacchetto NuGet Microsoft.Bcl).
        // Questa volta, il compilatore esegue tutto il lavoro, quindi non
        // c'è sovraccarico di runtime. Con questo approccio, il metodo diventa:

        private void RaiseNotifyPropertyChanged<T>([CallerMemberName] string propertyName = "")
        {
            // Fa quealcosa
        }

        // Ottenere il nome della proprietà con nameof: l'attributo CallerMemberName è stato
        // probabilmente creato su misura per questo caso d'uso (aumentando PropertyChanged
        // in una classe base), ma in C# 6 il team del compilatore ha finalmente fornito
        // qualcosa di molto più utile:
        //  la parola chiave nameof.
        // Nameof è utile per molti scopi; in questo caso, se sostituisco il codice basato
        // su espressioni con nameof, ancora una volta il compilatore farà tutto il
        // lavoro (nessun overhead di runtime).
        // Vale la pena notare che si tratta strettamente di una funzionalità della versione
        // del compilatore e non di una funzionalità della versione .NET:
        // è possibile utilizzare questa tecnica e continuare a essere destinata
        // a .NET Framework 2.0. Tuttavia, tu (e tutti i membri del tuo team) devi usare
        // almeno Visual Studio 2015.
        // L'uso di nameof è simile al seguente:

        public int UnreadItemCountType2
        {
            get
            {
                return m_unreadItemCount;
            }
            set
            {
                m_unreadItemCount = value;
                RaiseNotifyPropertyChanged(nameof(UnreadItemCount));
            }
        }


    }

    // Esempio 3
    public interface IRaisePropertyChanged
    {
        public void RaisePropertyChanged(string propertyName);
    }

    public class DataBindingObject3<T> : IRaisePropertyChanged 
        where T : IEqualityComparer
    {

        private string Name;
        T m_value;
        private IRaisePropertyChanged m_owner;

        public DataBindingObject3(string name, IRaisePropertyChanged prop)
        {
            Name = name;
            m_owner = prop;
        }

        public void SetValue(T newValue)
        {
            if (newValue != m_value)
            {
                m_value = newValue;
                m_owner.RaisePropertyChanged(this.Name);
            }
        }

        // PART 1: required for any class that implements INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            // In C# 6, you can use PropertyChanged?.Invoke.
            // Otherwise I'd suggest an extension method.
            var toRaise = PropertyChanged;
            if (toRaise != null)
                toRaise(this, args);
        }
        // PART 2: IRaisePropertyChanged-specific
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        // This method is only really for the sake of the interface,
        // not for general usage, so I implement it explicitly.
        void IRaisePropertyChanged.RaisePropertyChanged(string propertyName)
        {
            this.RaisePropertyChanged(propertyName);
        }

    }

    public class NotifyProperty<T>
    {
        private IRaisePropertyChanged owner;
        private object p;
        private T initialValue;

        public NotifyProperty(IRaisePropertyChanged owner, object p, T initialValue)
        {
            this.owner = owner;
            this.p = p;
            this.initialValue = initialValue;
        }
    }

    public static class NotifyChangedExtension
    {
        public static object ObjectNamingExtensions { get; private set; }

        // Mettere insieme i pezzi: ora ho l'interfaccia che il modello di visualizzazione
        // deve implementare e la classe NotifyProperty utilizzata per le proprietà che
        // saranno associate ai dati. L'ultimo passaggio consiste nella costruzione di
        // NotifyProperty; per questo, è ancora necessario passare il nome di una proprietà,
        // in qualche modo. Se sei abbastanza fortunato da usare C # 6, questo è facile
        // da fare con l'operatore nameof. In caso contrario, puoi invece creare
        // NotifyProperty con l'aiuto di espressioni, ad esempio utilizzando un metodo
        // di estensione (sfortunatamente, non c'è nessun posto per CallerMemberName
        // per aiutare questa volta):
        public static NotifyProperty<T> CreateNotifyProperty<T>(
            this IRaisePropertyChanged owner,
            Expression<Func<T>> nameExpression, T initialValue)
        {
            return new NotifyProperty<T>(owner,
            ObjectNamingExtensions.GetName(nameExpression),
            initialValue);
        }
        // Listing of GetName provided earlier

    }

    // Esempio 4
    public class LogInViewModel : IRaisePropertyChanged
    {
        public LogInViewModel()
        {
            // C# 6
            this.m_userNameProperty = new NotifyProperty<string>(
         this, nameof(UserName), null);
            // Extension method using expressions
            this.m_userNameProperty = this.CreateNotifyProperty(() => UserName, null);
        }
        private readonly NotifyProperty<string> m_userNameProperty;
        public string UserName
        {
            get
            {
                return m_userNameProperty.Value;
            }
            set
            {
                m_userNameProperty.SetValue(value);
            }
        }
        // Plus the IRaisePropertyChanged code in Figure 1 (otherwise, use a base class)
    }


    // Esempio 5

    public interface IProperty<TValue>
    {
    }

    public interface INotifyValueChanged
    {
        Action<object, object> ValueChanged { get; set; }
    }

    internal class ValueChangedEventArgs
    {
    }

    public class DerivedNotifyProperty<TValue> : IProperty<TValue>
    {
        private readonly IRaisePropertyChanged m_owner;
        private readonly Func<TValue> m_getValueProperty;
        public DerivedNotifyProperty(IRaisePropertyChanged owner,
        string derivedPropertyName, Func<TValue> getDerivedPropertyValue,
        params INotifyValueChanged[] valueChangesToListenFor)
        {
            this.m_owner = owner;
            this.Name = derivedPropertyName;
            this.m_getValueProperty = getDerivedPropertyValue;
            this.m_value = new Lazy<TValue>(m_getValueProperty);
            foreach (INotifyValueChanged valueChangeToListenFor in valueChangesToListenFor)
                valueChangeToListenFor.ValueChanged += (sender, e) => RefreshProperty();
        }
        // Name property and ValueChanged event omitted for brevity 
        private Lazy<TValue> m_value;
        public TValue Value
        {
            get
            {
                return m_value.Value;
            }
        }

        public string Name { get; private set; }

        public void RefreshProperty()
        {
            // Ensure we retrieve the value anew the next time it is requested
            this.m_value = new Lazy<TValue>(m_getValueProperty);
            OnValueChanged(new ValueChangedEventArgs());
            m_owner.RaisePropertyChanged(Name);
        }

        private void OnValueChanged(ValueChangedEventArgs valueChangedEventArgs)
        {
            // Fa qualcosa;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
