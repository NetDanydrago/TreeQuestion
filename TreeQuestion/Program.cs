using System;

namespace TreeQuestion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DecisionTreeTest();
        }

        static private void DecisionTreeTest()
        {
            Console.WriteLine("Please, answer a few questions with yes or no.");
            Console.WriteLine();
            MakeDecisionTree().Evaluate();
            Console.WriteLine("Hola Mundo");

        }
        static private bool GetUserAnswer(string question)
        {
            var result = false;
            Console.WriteLine(question);
            string userInput;
                userInput = Console.ReadLine().ToLower();
                if (userInput == "si")
                    result =  true;
                if (userInput == "no")
                    result = false;
            return result;
        }

        static private DecisionTreeQuery MakeDecisionTree()
        {
            var queryNasalComun =
             new DecisionTreeQuery("¿Presenta goteo nasal?",
               new DecisionTreeResult("Posible caso de resfriado comun"),
                   new DecisionTreeResult("Consulte un medico?"), GetUserAnswer);

            var queryNasalAlergia =
                 new DecisionTreeQuery("¿Presenta goteo nasal?",
                   new DecisionTreeResult("Posible caso de alergia"),
                       new DecisionTreeResult("Consulte un medico?"), GetUserAnswer);

            var queryEstorComun =
                 new DecisionTreeQuery("¿Tiene Estornudos?",
                   queryNasalComun,
                       new DecisionTreeResult("Consulte un medico?"), GetUserAnswer);

            var queryEstorAlergia =
                  new DecisionTreeQuery("¿Tiene Estornudos?",
                    queryNasalAlergia,
                        new DecisionTreeResult("Consulte un medico?"), GetUserAnswer);

            var queryOjos =
                new DecisionTreeQuery("¿Tiene ojos irritados?",
                   queryEstorAlergia,
                        queryEstorComun, GetUserAnswer);

            var queryGripeTox =
                 new DecisionTreeQuery("¿Presentas Tos?",
                 new DecisionTreeResult("Posible caso de gripe"),
                         new DecisionTreeResult("Ve a un Medico"), GetUserAnswer);

            var queryCoroAsistencia =
                 new DecisionTreeQuery("¿Has asistido a reunio mas de 20 personas en espacios cerrados?",
                new DecisionTreeResult("Posible caso de coronavirus"),
                        new DecisionTreeResult("Ve a un Medico"), GetUserAnswer);

            var queryCoroTox =
                new DecisionTreeQuery("¿Presentas Tos?",
                    queryCoroAsistencia,
                        new DecisionTreeResult("Ve a un Medico"), GetUserAnswer);

            var queryCoroFatiga =
              new DecisionTreeQuery("¿Presenta debilida o fatiga?",
                queryCoroTox,
                        new DecisionTreeResult("Ve a un Medico"), GetUserAnswer);

            var queryGripe =
              new DecisionTreeQuery("¿Presenta debilida o fatiga?",
                queryGripeTox,
                        new DecisionTreeResult("Ve a un Medico"), GetUserAnswer);

            var queryAire =
              new DecisionTreeQuery("¿Esperimenta falta de aire?",
                  queryCoroFatiga,
                      queryGripe, GetUserAnswer);

            var queryFiebreMayor =
                new DecisionTreeQuery("¿Presenta fiebre mayor a 38ºC?",                       
                            queryAire,
                             queryOjos, 
                                GetUserAnswer);
            return queryFiebreMayor;
        }

        abstract public class DecisionTreeCondition
        {
            protected string Sentence { get; private set; }
            abstract public void Evaluate();
            public DecisionTreeCondition(string sentence)
            {
                Sentence = sentence;
            }
        }

        public class DecisionTreeQuery : DecisionTreeCondition
        {
            private DecisionTreeCondition Positive;
            private DecisionTreeCondition Negative;
            private Func<string, bool> UserAnswerProvider;
            public override void Evaluate()
            {
                if (UserAnswerProvider(Sentence))
                    Positive.Evaluate();
                else
                    Negative.Evaluate();
            }
            public DecisionTreeQuery(string sentence,
                                     DecisionTreeCondition positive,
                                     DecisionTreeCondition negative,
                                     Func<string, bool> userAnswerProvider)
              : base(sentence)
            {
                Positive = positive;
                Negative = negative;
                UserAnswerProvider = userAnswerProvider;
            }
        }

        public class DecisionTreeResult : DecisionTreeCondition
        {
            public override void Evaluate()
            {
                Console.WriteLine(Sentence);
            }
            public DecisionTreeResult(string sentence)
              : base(sentence)
            {
            }
        }
    }
}
