using System;

namespace DotNetDesignPatternDemos.Architectural.Decisional.ADR
{
    /*
     * Record di decisioni architetturali (ADR)
     * 
     * Una decisione architetturale (AD) è una scelta di progettazione software che risponde 
     * a un requisito funzionale o non funzionale che è architettonicamente significativo. 
     * 
     * Un requisito architettonicamente significativo (ASR) è un requisito che ha un effetto 
     * misurabile sull'architettura e sulla qualità di un sistema software. 
     * 
     * Un Record di decisione architetturale (ADR) acquisisce un singolo AD, come spesso accade 
     * quando si scrivono note personali o verbali di riunione; la raccolta di ADR creati e 
     * mantenuti in un progetto costituisce il suo registro delle decisioni. 
     * 
     * Tutti questi sono all'interno del tema dell'Architectural Knowledge Management (AKM), ma 
     * l'uso di ADR può essere esteso alla progettazione e ad altre decisioni 
     * ("qualsiasi record di decisione").
     * 
     * L'obiettivo dell'organizzatore adr presente su GitHub è di:
     * 
     *      Motivare la necessità e i vantaggi dell'acquisizione dell'AD e stabilire un 
     *      vocabolario comune.
     *      
     *      Rafforzare gli strumenti intorno alle ADR, a supporto di pratiche agili e processi 
     *      di ingegneria del software iterativi e incrementali.
     *      
     *      Fornire indicazioni alla conoscenza pubblica nel contesto di AKM e ADR 
     *      (ad esempio, questo sito Web)
     * 
     * Sito ufficiale .: https://adr.github.io/
     * 
     * Per definizione .:
     * 
     *      Esiste una Relazione stretta tra ADR, MADR e altri tipi di Desisionale Makers
     *      
     *  Tools
     *  
     *      ADMentor Componente aggiuntivo per la modellazione delle decisioni architetturali per 
     *      Sparx Enterprise Architect
     *      
     *      adr-tools - script bash per gestire ADR nel formato Nygard.
     *      
     *      Script Ansible per installare adr-tools: ansible-adr-tools
     *      
     *      Riscrittura in C#: adr-cli
     *      Riscrittura Java: adr-j
     *      Riscrittura .js nodo: adr
     *      Versione PHP: phpadr
     *      Modulo Powershell: adr-ps
     *      Riscrittura Python: adr-tools-python
     *      
     *      Un altro modulo Powershell: ArchitectureDecisionRecords
     *      
     *      adr-log: genera un registro delle decisioni architetturali da MADR.
     *      
     *      adr-manager: crea modelli MADR direttamente nel browser Web.
     *      
     *      adr-viewer - applicazione python per generare un sito Web da un insieme di ADR.
     *      
     *      Embedded Architectural Decision Records, che mostra come un log AD distribuito 
     *      può essere incorporato nel codice Java tramite annotazioni ADR.
     *      
     *      Log4brains: CLI e interfaccia utente Web per registrare e pubblicare le ADR 
     *      come sito Web statico
     *      
     *      architectural-decision: libreria PHP per creare ADR utilizzando gli attributi PHP8.
     * 
     * 
     *      // Github progetto ADR : https://github.com/joelparkerhenderson/architecture-decision-record#what-is-an-architecture-decision-record
     * 
     *      Ci sono già dei template pronti con esempi per comprendere meglio la definizione
     *      delle decisioni e quali applicate.
     *      
     *      Examples:
     *          CSS framework
     *          Environment variable configuration
     *          Metrics, monitors, alerts
     *          Microsoft Azure DevOps
     *          Monorepo vs multirepo
     *          Programming languages
     *          Secrets storage
     *          Timestamp format
     * 
     *      // Github progetto MADR : https://github.com/adr/madr
     *      
     *      E' interessante poi trovare questo tool facile da usare che fà da manager nel 
     *      contesto di questo modello.
     *      
     *          // Immagine 1 : main-webview.png
     *          
     *      Ed è disponibile per Vs code.
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
