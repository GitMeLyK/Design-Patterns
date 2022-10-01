using System;

namespace DotNetDesignPatternDemos.Concurrency.ProducerConsumer
{
    /*
     * 
     * Uno dei modelli più comuni nella programmazione in thread è il modello produttore-consumatore.
     * L'idea è di elaborare i dati in modo asincrono partizionando le richieste tra diversi gruppi 
     * di thread. Il produttore è un thread che genera richieste da elaborare. 
     * Il consumatore è un filo che prende quelle richieste e agisce su di esse. 
     * Questo modello fornisce una separazione netta che consente una migliore progettazione 
     * del thread e semplifica lo sviluppo e il debug. 
     * Ciò consente di scambiare diversi produttori senza incidere sul consumatore. 
     * Ci consente inoltre di avere più produttori serviti da un singolo consumatore 
     * o più consumatori che servono un singolo produttore. 
     * Più in generale, possiamo variare il numero di entrambi in base alle esigenze 
     * di prestazione o ai requisiti dell'utente . 
     * Il produttore fa affidamento sul consumatore per fare spazio nell'area dati in modo che 
     * possa inserire più informazioni mentre, allo stesso tempo, il consumatore fa affidamento 
     * sul produttore di inserire informazioni nell'area dati in modo che possa rimuovere tali 
     * informazioni. Ne consegue quindi che è necessario un meccanismo che consenta al produttore 
     * e al consumatore di comunicare in modo che sappiano quando è sicuro tentare di scrivere 
     * o leggere informazioni dall'area dati dell'ordine. Supponiamo di voler scrivere 
     * un'applicazione che accetti i dati mentre li elabora nell'ordine in cui sono stati ricevuti. 
     * Poiché mettere in coda (produzione) questi dati è molto più veloce dell'elaborazione 
     * (consumo) effettiva, il modello di progettazione Produttore/Consumatore è più adatto per 
     * questa applicazione . 
     * Si potrebbe plausibilmente mettere sia il produttore che il consumatore nello stesso ciclo 
     * per questa applicazione, ma la coda di elaborazione non sarà in grado di aggiungere dati 
     * aggiuntivi fino a quando non sarà terminata l'elaborazione del primo pezzo di dati. 
     * L'approccio Producer/Consumerpattern a questa applicazione consiste nell'accodare i dati 
     * nel ciclo del produttore e fare in modo che l'elaborazione effettiva venga eseguita nel 
     * ciclo del consumatore. 
     * Ciò in effetti consentirà al ciclo del consumatore di elaborare i dati al proprio ritmo, 
     * consentendo al ciclo del produttore di accodare dati aggiuntivi contemporaneamente.
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
