using System;

namespace DotNetDesignPatternDemos.Architectural.EventSource
{
    /*
     *  Un modello di progettazione per eventi appunto l'EventSource in genere deve aggiornare 
     *  il database e inviare messaggi/eventi. 
     *  Ad esempio, un servizio che partecipa a una saga deve aggiornare atomicamente il database 
     *  e inviare messaggi/eventi. Analogamente, un servizio che pubblica un evento di dominio deve 
     *  aggiornare atomicamente un'aggregazione e pubblicare un evento.
     *  
     *  Un servizio deve aggiornare atomicamente il database e inviare messaggi per evitare 
     *  incoerenze e bug dei dati. Tuttavia, non è possibile utilizzare una transazione 
     *  distribuita tradizionale (2PC) che si estende sul database e sul broker di messaggi 
     *  per aggiornare atomicamente il database e pubblicare messaggi/eventi. 
     *  
     *  Il broker di messaggi potrebbe non supportare 2PC. 
     *  E anche se lo fa, è spesso indesiderabile accoppiare il servizio sia al database che al messaggio.
     *  
     *  Ma senza usare 2PC, l'invio di un messaggio nel mezzo di una transazione non è affidabile. 
     *  Non vi è alcuna garanzia che la transazione si impegnerà. 
     *  Allo stesso modo, se un servizio invia un messaggio dopo aver eseguito il commit della 
     *  transazione, non vi è alcuna garanzia che non si arresti in modo anomalo prima di inviare 
     *  il messaggio.
     *  
     *  Inoltre, i messaggi devono essere inviati al broker di messaggi nell'ordine in cui sono 
     *  stati inviati dal servizio. Di solito devono essere consegnati a ciascun consumatore nello 
     *  stesso ordine, anche se questo è al di fuori dell'ambito di questo modello. 
     *  Ad esempio, supponiamo che un aggregato venga aggiornato da una serie di transazioni , ecc. 
     *  
     *  Queste transazioni possono essere eseguite dalla stessa istanza del servizio o da istanze 
     *  del servizio diverse. Ogni transazione pubblica un evento corrispondente: ,ecc. 
     *  
     *  Poiché precede , l'evento deve essere pubblicato prima.
     *  
     *  Problema
     *      Come aggiornare in modo affidabile/atomico il database e inviare messaggi/eventi?
     *      
     *  Come
     *      2PC non è un'opzione
     *      Se la transazione del database esegue il commit, è necessario inviare messaggi. 
     *      Al contrario, se il database esegue il rollback, i messaggi non devono essere inviati
     *      I messaggi devono essere inviati al broker di messaggi nell'ordine in cui sono stati 
     *      inviati dal servizio. Questo ordine deve essere mantenuto in più istanze del servizio 
     *      che aggiornano la stessa aggregazione.
     *      
     *  Soluzione
     *      Una buona soluzione a questo problema consiste nell'utilizzare l'origine eventi. 
     *      L'approvvigionamento di eventi mantiene lo stato di un'entità aziendale, ad esempio 
     *      un Ordine o un Cliente, come sequenza di eventi che cambiano stato. 
     *      Ogni volta che cambia lo stato di un'entità aziendale, un nuovo evento viene aggiunto 
     *      all'elenco degli eventi. 
     *      Poiché il salvataggio di un evento è una singola operazione, è intrinsecamente atomico. 
     *      L'applicazione ricostruisce lo stato corrente di un'entità riproducendo gli eventi.
     *      
     * Le applicazioni mantengono gli eventi in un archivio eventi, che è un database di eventi. 
     * L'archivio dispone di un'API per l'aggiunta e il recupero degli eventi di un'entità. 
     * L'archivio eventi si comporta anche come un broker di messaggi. 
     * Fornisce un'API che consente ai servizi di sottoscrivere eventi. 
     * Quando un servizio salva un evento nell'archivio eventi, questo viene recapitato a tutti 
     * gli abbonati interessati.
     * 
     * Alcune entità, ad esempio un cliente, possono avere un numero elevato di eventi. 
     * Per ottimizzare il caricamento, un'applicazione può salvare periodicamente uno snapshot 
     * dello stato corrente di un'entità. 
     * Per ricostruire lo stato corrente, l'applicazione trova lo snapshot più recente e gli 
     * eventi che si sono verificati dopo tale snapshot. 
     * Di conseguenza, ci sono meno eventi da riprodurre.
     * 
     * Esempio
     *  Customers and Orders è un esempio di applicazione creata utilizzando Event Sourcing e CQRS. 
     *  L'applicazione è scritta in Java e utilizza Spring Boot. 
     *  È costruito utilizzando Eventuate, che è una piattaforma applicativa basata 
     *  sull'approvvigionamento di eventi e CQRS.
     *  
     *  Nel diagramma figura 1 in allegato viene illustrato come vengono mantenuti gli ordini.
     *  
     *  Invece di archiviare semplicemente lo stato corrente di ogni ordine come riga in una 
     *  tabella, l'applicazione persiste ciascuno come sequenza di eventi.
     *  Può sottoscrivere gli eventi dell'ordine e aggiornare il proprio 
     *  stato.ORDERSOrderCustomerService
     *  
     *  Nel codice sotto vedere la classe Order e il CusgtomerWorkFlow 
     *  
     *  Elabora un evento tentando di riservare credito per il cliente degli ordini.OrderCreated
     *  
     *  Esistono diverse applicazioni di esempio che illustrano come utilizzare l'origine eventi.
     *  
     *  Contesto risultante
     *      L'approvvigionamento di eventi presenta diversi vantaggi:
     *          
     *          - Risolve uno dei problemi chiave nell'implementazione di un'architettura 
     *          basata su eventi e consente di pubblicare in modo affidabile gli eventi ogni 
     *          volta che lo stato cambia.
     *          
     *          - Poiché mantiene gli eventi anziché gli oggetti di dominio, evita principalmente 
     *          il problema della mancata corrispondenza dell'impedenza relazionale degli oggetti.
     *          
     *          - Fornisce un registro di controllo affidabile al 100% delle modifiche apportate 
     *          a un'entità aziendale.
     *          
     *          - Consente di implementare query temporali che determinano lo stato di un'entità 
     *          in qualsiasi momento.
     *          
     *          - La logica di business basata sull'approvvigionamento di eventi è costituita 
     *          da entità aziendali ad accoppiamento debole che si scambiano eventi. 
     *          In questo modo è molto più semplice eseguire la migrazione da un'applicazione 
     *          monolitica a un'architettura di microservizi.
     *          
     *          - L'approvvigionamento degli eventi presenta anche diversi inconvenienti:
     *              - È uno stile di programmazione diverso e sconosciuto e quindi c'è una 
     *              curva di apprendimento.
     *              - L'archivio eventi è difficile da interrogare poiché richiede query 
     *              tipiche per ricostruire lo stato delle entità aziendali. 
     *              È probabile che sia complesso e inefficiente. 
     *              Di conseguenza, l'applicazione deve utilizzare CQRS (Command Query Responsibility Segregation) 
     *              per implementare le query. 
     *              Ciò a sua volta significa che le applicazioni devono gestire dati coerenti.  
     *  
     */

    /*
     * Code Java

        public class Order extends ReflectiveMutableCommandProcessingAggregate<Order, OrderCommand> {

          private OrderState state;
          private String customerId;

          public OrderState getState() {
            return state;
          }

          public List<Event> process(CreateOrderCommand cmd) {
            return EventUtil.events(new OrderCreatedEvent(cmd.getCustomerId(), cmd.getOrderTotal()));
          }

          public List<Event> process(ApproveOrderCommand cmd) {
            return EventUtil.events(new OrderApprovedEvent(customerId));
          }

          public List<Event> process(RejectOrderCommand cmd) {
            return EventUtil.events(new OrderRejectedEvent(customerId));
          }

          public void apply(OrderCreatedEvent event) {
            this.state = OrderState.CREATED;
            this.customerId = event.getCustomerId();
          }

          public void apply(OrderApprovedEvent event) {
            this.state = OrderState.APPROVED;
          }


          public void apply(OrderRejectedEvent event) {
            this.state = OrderState.REJECTED;
          }
        Di seguito è riportato un esempio di un gestore eventi nella sottoscrizione degli eventi:CustomerServiceOrder

        @EventSubscriber(id = "customerWorkflow")
        public class CustomerWorkflow {

          @EventHandlerMethod
          public CompletableFuture<EntityWithIdAndVersion<Customer>> reserveCredit(
                  EventHandlerContext<OrderCreatedEvent> ctx) {
            OrderCreatedEvent event = ctx.getEvent();
            Money orderTotal = event.getOrderTotal();
            String customerId = event.getCustomerId();
            String orderId = ctx.getEntityId();

            return ctx.update(Customer.class, customerId, new ReserveCreditCommand(orderTotal, orderId));
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
