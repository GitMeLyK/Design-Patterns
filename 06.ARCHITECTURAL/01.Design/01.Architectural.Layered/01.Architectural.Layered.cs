using System;

namespace DotNetDesignPatternDemos.Architectural.Layered
{
    /*
     * Idioma.:
     * 
     * Ti sei mai chiesto come Google fa funzionare Gmail in diverse lingue in tutto il mondo? 
     * Gli utenti possono utilizzare Gmail ogni giorno in inglese, spagnolo, francese, russo e 
     * molte altre lingue.
     * Google ha sviluppato diverse applicazioni Gmail per ogni paese? Certo che no. 
     * Hanno sviluppato una versione interna che esegue tutta l'elaborazione dei messaggi e 
     * quindi hanno sviluppato diverse interfacce utente esterne che funzionano in molte lingue.
     * Google ha sviluppato l'applicazione Gmail in diversi livelli:
     *      - C'è uno strato interno che fa tutta l'elaborazione.
     *      - C'è un livello esterno che comunica con gli utenti nella loro lingua.
     *      - C'è anche un altro livello che interagisce con un database in cui sono 
     *        archiviati i messaggi di posta elettronica degli utenti (milioni o forse miliardi).
     * Gmail è diviso in almeno tre livelli, ognuno di essi ha una missione ed esistono 
     * separatamente per gestire processi diversi a diversi livelli. È un eccellente esempio 
     * di architettura a strati.
     * 
     * Come puoi vedere nel diagramma sopra, un'architettura a livelli standard ha cinque parti:
     * 
     *      - Livello di interazione dell'utente: Questo è il livello che interagisce con 
     *      gli utenti attraverso schermate, moduli, menu, report, ecc. È il livello più 
     *      visibile dell'applicazione. Definisce l'aspetto dell'applicazione.
     *      
     *      - Livello di funzionalità: Questo è il livello che presenta le funzioni, i 
     *      metodi e le procedure del sistema in base al livello delle regole di business. 
     *      Determina come funzionano i menu a discesa, come funzionano i pulsanti e come 
     *      il sistema naviga tra le schermate.
     *      
     *      - Livello delle regole di business: Questo livello contiene regole che determinano 
     *      il comportamento dell'intera applicazione, ad esempio "Se viene stampata una fattura, 
     *      inviare un'e-mail al cliente, selezionare tutti gli articoli venduti e ridurne 
     *      le scorte nel modulo di gestione delle scorte".
     *      
     *      - Livello principale dell'applicazione: Questo server contiene i programmi principali, 
     *      le definizioni di codice e le funzioni di base dell'applicazione. I programmatori 
     *      lavorano in questo livello per la maggior parte del tempo.
     *      
     *      - Livello del database: Questo livello contiene le tabelle, gli indici e i dati 
     *      gestiti dall'applicazione. Le ricerche e le operazioni di inserimento/eliminazione
     *      /aggiornamento vengono eseguite qui.
     *      
     *  Come Funziona?
     *  
     *      - Un sistema ERP (contabilità fornitori, contabilità clienti, gestione delle scorte, 
     *      gestione delle risorse umane, gestione della produzione, gestione dei fornitori, 
     *      acquisti, tesoreria, finanza, contabilità, ecc.) ha un livello di interazione 
     *      dell'utente per ogni modulo: schermate, moduli, menu, report. Questo è ciò che 
     *      l'utente vede e ciò che usa.
     *      
     *      - Il livello di funzionalità naviga attraverso i diversi moduli, presenta 
     *      sequenze di schermate all'utente ed esegue tutte le operazioni di input dei dati.
     *      
     *      - Il livello delle regole aziendali determina il comportamento dei moduli dell'ERP: 
     *      "Se un nuovo dipendente viene inserito nei moduli HR e payroll, inserire un corso 
     *      introduttivo nel menu di formazione del dipendente".
     *      
     *      - Il livello principale dell'applicazione è il luogo in cui si trova tutto il 
     *      codice di sistema. È qui che gli sviluppatori aggiungono personalizzazioni 
     *      e nuove funzionalità.
     *      
     *      - Il livello di database contiene le tabelle, gli indici e i dati gestiti da 
     *      ciascuno dei moduli.
     *      
     *  Vantaggi:
     *      - I livelli sono autonomi: un gruppo di modifiche in un livello non influisce 
     *      sugli altri. Questo è un bene perché possiamo aumentare la funzionalità di un 
     *      livello, ad esempio, rendendo un'applicazione che funziona solo su PC per 
     *      funzionare su telefoni e tablet, senza dover riscrivere l'intera applicazione.
     *      
     *      - I livelli consentono una migliore personalizzazione del sistema.
     *      
     * Svantaggi:
     *      - I livelli rendono un'applicazione più difficile da mantenere. Ogni modifica 
     *      richiede un'analisi.
     *      
     *      -I layer possono influire sulle prestazioni dell'applicazione perché creano 
     *      un sovraccarico durante l'esecuzione: ogni layer nei livelli superiori deve 
     *      connettersi a quelli nei livelli inferiori per ogni operazione nel sistema.
     *      
     * Nota:
     *      - Non tutte le applicazioni valgono la manutenzione aggiuntiva richiesta 
     *      per l'architettura a più livelli.
     * 
     * -------------------------------------------------------------------------------------
     * 
     * Recup:
     * 
     *  Modello di architettura Stratificato
     *  
     *      Descrizione
     *          - Il software opera in strati che consentono a ciascun componente di 
     *          essere indipendente dal resto.
     *          
     *      Vantaggi
     *          - Incapsulamento di hardware, software e funzionalità.
     *          - Se un livello viene modificato, il resto dei livelli rimane lo stesso.
     *
     *      Difetto
     *          - Per le applicazioni di piccole dimensioni, molti livelli creano un 
     *          problema di prestazioni e sono molto difficili da mantenere.
     *          
     *      Quando Usarlo?
     *          Solo per grandi applicazioni.
     */

    /*
     * Caso Reale
     * 
     * Vediamo come è stata utilizzata un'architettura a più livelli per risolvere un problema reale.
     * 
     *      Amaze è una società di software di gestione dei progetti. 
     *      Il loro prodotto è venduto a livello globale con un modello pay-per-user mensile e 
     *      ampiamente noto tra la comunità di gestione dei progetti per essere facile da usare 
     *      e in grado di operare su molti dispositivi diversi (PC, notebook, laptop, tablet, 
     *      iPhone, iPad e telefoni Android).
     * 
     * Qual è il problema aziendale?
     * 
     *      Il problema aziendale è molto semplice: Amaze deve funzionare su qualsiasi 
     *      dispositivo popolare sul mercato ed essere in grado di supportare i dispositivi futuri. 
     *      Deve esistere una sola versione del software per tutti i dispositivi. Nessun caso 
     *      particolare, nessuna eccezione consentita.
     *      
     * Quindi, per riassumere:
     *      
     *      Sappiamo che gli utenti hanno dispositivi diversi.
     *      Ci deve essere una sola applicazione software perché l'azienda vuole avere bassi 
     *      costi di manutenzione del software.
     *      Quando viene lanciato un nuovo dispositivo, non vogliamo cambiare l'intero 
     *      prodotto software.
     *      
     * Qual è la soluzione?
     * 
     *      Ora che abbiamo attraversato il processo di pensiero, vediamo il diagramma 
     *      dettagliato dell'architettura: 
     *      // Figura 2 Layered_Amaze.png
     *      
     *      Nel nostro caso, il livello delle regole di base di Amaze è fondamentale. 
     *      Questo livello contiene regole che determinano il comportamento dell'intera 
     *      applicazione, ad esempio "È possibile creare una pianificazione del progetto 
     *      solo se l'ambito del progetto è definito". 
     *      L'intelligenza del prodotto è in questo livello: 
     *       tutte le caratteristiche speciali che provengono da decenni di esperienza 
     *       nei progetti sono sviluppate lì. Il livello principale dell'applicazione 
     *       sarà la parte più significativa del codice dell'applicazione.
     */

    /*
     * Analisi Tipo:
     * 
     * Contesto: Sei stato un architetto software presso un'importante compagnia assicurativa 
     * nel tuo paese negli ultimi quattro anni. Il tuo capo è Carla, il Chief Information 
     * Officer (CIO) dell'azienda. L'azienda ha uffici in 24 città e più di 2.100 dipendenti.
     * 
     * La tua missione: Ti è stato chiesto di sviluppare un sistema software per la gestione 
     * di nuove polizze assicurative per il personale dei tuoi clienti in diversi 
     * settori (finanza, produzione, tecnologia, ingegneria, ecc.). Ogni settore ha 
     * costumi e requisiti diversi per la gestione dei benefici per i dipendenti.
     * 
     * Ecco alcune domande chiave che dovresti porti:
     * 
     *  - Come gestirai i diversi requisiti del settore in un unico sistema?
     *  
     *   - Cosa hanno in comune tutte le polizze in tutti i settori, indipendentemente 
     *   dal settore, dal cliente o dalle merci assicurate?
     *   
     *   - Come è possibile separare i requisiti comuni per tutti i settori e i 
     *   requisiti specifici per ciascun settore?
     *   
     *   - Chi definirà le regole di business per ogni settore?
     *   
     *   - È possibile produrre un diagramma dell'architettura per questa impostazione aziendale?
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
