using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class UserFeedback
    {
        public int TemplateID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public List<feedbackQuestions> questions { get; set; } = new List<feedbackQuestions>();

        public string AnswerList { get; set; }
    }


    public class feedbackQuestions
    {
        public int QuestionID { get; set; }

        public Int16 SrNo { get; set; }

        public string QuestionText { get; set; }

        public List<QuestionOptions> options { get; set; } = new List<QuestionOptions>();

        public string  AnswerType { get; set; }

        public string AnswerText { get; set; }

        public int? AnswerID { get; set; }
    }

    public class QuestionOptions
    {
        public int OptionID { get; set; }

        public Int16 SrNo { get; set; }

        public string OptionDescription { get; set; }

        public string OptionImgUrl { get; set; }

        public bool IsCheck { get; set; }

        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class UserFeedbackStatus
    {
        public string Status { get; set; }
        public int TemplateID { get; set; }
    }


}