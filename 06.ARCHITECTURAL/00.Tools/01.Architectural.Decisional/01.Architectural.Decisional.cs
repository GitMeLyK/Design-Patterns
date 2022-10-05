using System;

namespace DotNetDesignPatternDemos.Architectural.Decisional
{
    /*
     * La progettazione dell'architettura software è un problema non di facile approccio,[4] 
     * quindi le decisioni architetturali sono difficili da ottenere correttamente. 
     * 
     * Spesso, non esiste un'unica soluzione ottimale per un determinato insieme di problemi 
     * di progettazione dell'architettura. 
     * 
     * Il processo decisionale architettonico è una responsabilità fondamentale degli architetti 
     * del software; ulteriori motivazioni per/dell'importanza delle decisioni architetturali come 
     * concetto di prima classe nell'architettura software possono essere trovate online.
     * 
     * In pratica, l'importanza di prendere le decisioni corrette è sempre stata riconosciuta, ad 
     * esempio nei processi di sviluppo software come OpenUP; esistono molti modelli e pratiche per 
     * la documentazione decisionale. 
     * 
     * Sette di questi modelli sono confrontati. Lo standard più recente per le descrizioni 
     * dell'architettura, ISO/IEC/IEEE 42010:2011 ha un'entità razionale dedicata e fornisce 
     * raccomandazioni dettagliate su quali decisioni architetturali acquisire e quali proprietà 
     * di una decisione architetturale registrare nel registro delle decisioni.
     * 
     * - Fasi di gestione delle decisioni
     * 
     * -- Identificazione delle decisioni
     * 
     *      Prima che una decisione possa essere presa, la necessità di una decisione deve essere 
     *      articolata: quanto è urgente e quanto è importante l'AD? Deve essere realizzato ora o 
     *      può aspettare fino a quando non si saprà di più sui requisiti e sul sistema in costruzione? 
     *      
     *      Sia l'esperienza personale che quella collettiva, così come i metodi e le pratiche di 
     *      progettazione riconosciuti, possono aiutare con l'identificazione delle decisioni; 
     *      è stato proposto che il team di sviluppo software Agile mantenga un backlog 
     *      decisionale che integri il backlog del prodotto del progetto. 
     *  
     * -- Processo 
     * 
     *      Esiste un certo numero di tecniche decisionali, sia quelle generali che quelle 
     *      specifiche del software e dell'architettura software, ad esempio la mappatura 
     *      del dialogo. Il processo decisionale di gruppo è un argomento di ricerca attivo.
     *      
     * -- Documentazione decisionale
     * 
     *      Esistono molti modelli e strumenti per l'acquisizione delle decisioni, sia nelle 
     *      comunità agili (ad esempio, i record decisionali dell'architettura di M. Nygard) 
     *      che nell'ingegneria del software e nei metodi di progettazione dell'architettura 
     *      (ad esempio, vedi i layout delle tabelle suggeriti da IBM UMF e da Tyree e Akerman 
     *      di CapitalOne. G. Fairbanks incluse la logica decisionale nel suo Haikus di 
     *      architettura di una pagina; la sua notazione fu in seguito evoluta in Y-statements. 
     *      Vedi per motivazione, esempi, confronti.
     *      
     * -- Adozione della decisione (esecuzione)
     * 
     *      Le decisioni architetturali sono utilizzate nella progettazione del software; quindi 
     *      devono essere comunicati e accettati dalle parti interessate del sistema che lo finanziano, 
     *      lo sviluppano e lo gestiscono. 
     *      Gli stili di codifica architettonicamente evidenti e le revisioni del codice incentrate 
     *      su problemi e decisioni architetturali sono due pratiche correlate.
     *      
     *      Le decisioni architetturali devono anche essere prese in considerazione quando si 
     *      modernizza un sistema software nell'evoluzione del software.
     *      
     * -- Condivisione delle decisioni (passaggio facoltativo)
     * 
     *      Molte decisioni architettoniche ricorrono tra i progetti; quindi, le esperienze con 
     *      decisioni passate, sia buone che cattive, possono essere preziose risorse 
     *      riutilizzabili quando si impiega una strategia esplicita di gestione della conoscenza.
     * 
     * -- Esempi
     * 
     *      Su progetti su larga scala, il numero di decisioni architettoniche da prendere può 
     *      superare le 100, tra cui:
     *      
     *      Selezione dello schema di stratificazione architettonica e delle responsabilità 
     *      dei singoli livelli (quando si adotta il modello a Livelli)
     *      
     *      Scelta della tecnologia di implementazione per livello, componente e connettore 
     *      (ad esempio, linguaggio di programmazione, formato del contratto di interfaccia, 
     *      XML vs JSON durante la progettazione di interfacce di integrazione e scambi di messaggi)
     *      
     *      Scelta dei framework di livello di presentazione sul lato client (ad esempio, 
     *      framework JavaScript) e sul lato server (ad esempio, framework .Net c# Java e PHP)
     *      
     *      Per altri esempi, fare riferimento ai cataloghi dei concetti di progettazione in 
     *      Attribute-Driven Design 3.0 e ai modelli di guida decisionale specifici del dominio.
     *      
     * -- Modelli
     * 
     *      Molti modelli sono stati suggeriti da architetti praticanti e da ricercatori di 
     *      architettura software. I repository GitHub come "Architecture decision record (ADR)"
     *      e "Markdown Architectural Decision Records" ne raccolgono molti, oltre a collegamenti 
     *      a strumenti e suggerimenti per la scrittura.
     *      
     * 

Questo è un esempio di una decisione presa, che è formattata secondo il modello di dichiarazione Y proposto in:[26]

"Nel contesto del servizio Web shop, di fronte alla necessità di mantenere i dati delle sessioni utente coerenti e aggiornati tra le istanze del negozio, abbiamo deciso per il Database Session State Pattern (e rispetto allo Stato sessione client o allo Stato sessione server)[27] per ottenere l'elasticità del cloud, accettando che un database di sessione debba essere progettato, implementato e replicato."
     * 
     * 
     * 
     * 
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
