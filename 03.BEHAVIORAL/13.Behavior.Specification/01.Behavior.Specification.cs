using System;

namespace DotNetDesignPatternDemos.Behavioral.Specification
{
    /*
     * Nell'esempio corrente viene usato questo modello di progettazione relativo
     * alle regole di business per le specifiche.
     * In questo caso vediamo come l'interfaccia che obbliga alla dichiaraizione
     * di una determinata specifica e alla sua combinazione per l'accessorio filtro.
     * 
     * Vediamo che quindi è possibile organizzare genericament un problema che altrimenti
     * avrebbe potuto portare a una serie di riscritture del codice voluminoso in ordine
     * di grandezza per quanto sono gli elemeenti di specifca da adottare per ogni richiesta.
     * 
     * 
     */

    public interface ISpecification
    {
        bool IsSatisfiedBy(object candidate);
        ISpecification And(ISpecification other);
        ISpecification AndNot(ISpecification other);
        ISpecification Or(ISpecification other);
        ISpecification OrNot(ISpecification other);
        ISpecification Not();
    }

    public abstract class CompositeSpecification : ISpecification
    {
        public abstract bool IsSatisfiedBy(object candidate);

        public ISpecification And(ISpecification other)
        {
            return new AndSpecification(this, other);
        }

        public ISpecification AndNot(ISpecification other)
        {
            return new AndNotSpecification(this, other);
        }

        public ISpecification Or(ISpecification other)
        {
            return new OrSpecification(this, other);
        }

        public ISpecification OrNot(ISpecification other)
        {
            return new OrNotSpecification(this, other);
        }

        public ISpecification Not()
        {
            return new NotSpecification(this);
        }
    }

    public class AndSpecification : CompositeSpecification
    {
        private ISpecification leftCondition;
        private ISpecification rightCondition;

        public AndSpecification(ISpecification left, ISpecification right)
        {
            leftCondition = left;
            rightCondition = right;
        }

        public override bool IsSatisfiedBy(object candidate)
        {
            return leftCondition.IsSatisfiedBy(candidate) && rightCondition.IsSatisfiedBy(candidate);
        }
    }

    public class AndNotSpecification : CompositeSpecification
    {
        private ISpecification leftCondition;
        private ISpecification rightCondition;

        public AndNotSpecification(ISpecification left, ISpecification right)
        {
            leftCondition = left;
            rightCondition = right;
        }

        public override bool IsSatisfiedBy(object candidate)
        {
            return leftCondition.IsSatisfiedBy(candidate) && rightCondition.IsSatisfiedBy(candidate) != true;
        }
    }

    public class OrSpecification : CompositeSpecification
    {
        private ISpecification leftCondition;
        private ISpecification rightCondition;

        public OrSpecification(ISpecification left, ISpecification right)
        {
            leftCondition = left;
            rightCondition = right;
        }

        public override bool IsSatisfiedBy(object candidate)
        {
            return leftCondition.IsSatisfiedBy(candidate) || rightCondition.IsSatisfiedBy(candidate);
        }
    }

    public class OrNotSpecification : CompositeSpecification
    {
        private ISpecification leftCondition;
        private ISpecification rightCondition;

        public OrNotSpecification(ISpecification left, ISpecification right)
        {
            leftCondition = left;
            rightCondition = right;
        }

        public override bool IsSatisfiedBy(object candidate)
        {
            return leftCondition.IsSatisfiedBy(candidate) || rightCondition.IsSatisfiedBy(candidate) != true;
        }
    }

    public class NotSpecification : CompositeSpecification
    {
        private ISpecification Wrapped;

        public NotSpecification(ISpecification x)
        {
            Wrapped = x;
        }

        public override bool IsSatisfiedBy(object candidate)
        {
            return !Wrapped.IsSatisfiedBy(candidate);
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
