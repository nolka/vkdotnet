using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Questions
{
    [Obsolete("Questions service currently is not in use at date 2012.03.23")]
    public class QuestionsFactory: BaseFactory
    {

        public QuestionsFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private List<QuestionEntry> buildQuestionEntryList(XmlDocument x)
        {
            XmlNodeList qNodes = x.SelectNodes("/response/question");
            if (qNodes.Count > 0)
            {
                List<QuestionEntry> qList = new List<QuestionEntry>();
                foreach (XmlNode qNode in qNodes)
                {
                    QuestionEntry question = new QuestionEntry();
                    XmlUtils.UseNode(qNode);
                    question.Id = XmlUtils.Int("qid");
                    question.UserId = XmlUtils.Int("uid");
                    question.Type = XmlUtils.Int("type");
                    question.Text = XmlUtils.String("text");
                    question.AnswersCount = XmlUtils.Int("answers_num");
                    question.LastPosterDate = XmlUtils.String("last_poster_date");
                    question.LastPosterId = XmlUtils.Int("last_poster_id");
                    question.LastPosterName = XmlUtils.String("last_poster_name");
                    question.Date = XmlUtils.String("date");
                    question.UserName = XmlUtils.String("name");
                    question.UserPhoto = XmlUtils.String("photo");
                    question.IsUserOnline = XmlUtils.Bool("online");
                    qList.Add(question);
                }
                return qList;
            }
            return null;
        }


        private List<QuestionAnswer> buildQuestionAnswersList(XmlDocument x)
        {
            XmlNodeList qNodes = x.SelectNodes("/response/answer");
            if (qNodes.Count > 0)
            {
                List<QuestionAnswer> qList = new List<QuestionAnswer>();
                foreach (XmlNode qNode in qNodes)
                {
                    QuestionAnswer answer = new QuestionAnswer();
                    XmlUtils.UseNode(qNode);
                    answer.Id = XmlUtils.Int("aid");
                    answer.QuestionId = XmlUtils.Int("qid");
                    answer.UserId = XmlUtils.Int("uid");
                    answer.Text = XmlUtils.String("text");
                    answer.Number = XmlUtils.Int("num");
                    answer.Date = XmlUtils.String("date");
                    answer.UserName = XmlUtils.String("name");
                    answer.UserPhoto = XmlUtils.String("photo");
                    answer.IsUserOnline = XmlUtils.Bool("online");
                    qList.Add(answer);
                }
                return qList;
            }
            return null;
        }

        private List<QuestionType> buildQuestionTypeList(XmlDocument x)
        {
            XmlNodeList qNodes = x.SelectNodes("/response/type");
            if (qNodes.Count > 0)
            {
                List<QuestionType> qList = new List<QuestionType>();
                foreach (XmlNode qNode in qNodes)
                {
                    QuestionType t = new QuestionType();
                    XmlUtils.UseNode(qNode);
                    t.Id = XmlUtils.Int("tid");
                    t.Name = XmlUtils.String("name");
                    qList.Add(t);
                }
                return qList;
            }
            return null;
        }

        private List<QuestionAnswerVoter> buildQuestionVotersList(XmlDocument x)
        {
            XmlNodeList qNodes = x.SelectNodes("/response/vote");
            if (qNodes.Count > 0)
            {
                List<QuestionAnswerVoter> qList = new List<QuestionAnswerVoter>();
                foreach (XmlNode qNode in qNodes)
                {
                    QuestionAnswerVoter av = new QuestionAnswerVoter();
                    XmlUtils.UseNode(qNode);
                    av.Id = XmlUtils.Int("vid");
                    av.VoterId = XmlUtils.Int("voter_id");
                    av.Date = XmlUtils.String("date");
                    av.Name = XmlUtils.String("name");
                    av.Photo = XmlUtils.String("photo");
                    av.IsUserOnline = XmlUtils.Bool("online");
                    qList.Add(av);
                }
                return qList;
            }
            return null;
        }

        public List<QuestionEntry> Get(object qid, QuestionSortMode? sort, int? needProfiles, string nameCase, int? count, int? offset)
        {
            this.Manager.Method("questions.get", new object[] { "sort", sort, "need_profiles", needProfiles, "name_case", nameCase, "count", count, "offset", offset });
            if (qid != null)
            {
                if (qid is int)
                {
                    this.Manager.Params("qid",qid);
                }
                if (qid is string)
                {
                    this.Manager.Params("uids", qid);
                }
            }
            XmlDocument x = this.Manager.Execute().GetResponseXml()
            if (x.InnerText.Equals(""))
            {
                return null;
            }

            return this.buildQuestionEntryList(x);
        }

        public List<QuestionEntry> GetOutbound( QuestionSortMode? sort, int? needProfiles, string nameCase, int? count, int? offset)
        {
            this.Manager.Method("questions.getOutbound", new object[] { "sort", sort, "need_profiles", needProfiles, "name_case", nameCase, "count", count, "offset", offset});

            XmlDocument x = this.Manager.Execute().GetResponseXml();
            if (x.InnerText.Equals(""))
            {
                return null;
            }
            return this.buildQuestionEntryList(x);
        }

        public List<QuestionAnswer> GetAnswers(int qid, QuestionSortMode? sort, int? needProfiles, int? count, int? offset)
        {
            this.Manager.Method("questions.getAnswers", new object[] { "qid", qid, "sort", sort, "need_profiles", needProfiles, "count", count, "offset", offset });

            XmlDocument x = this.Manager.Execute().GetResponseXml();
            if (x.InnerText.Equals(""))
            {
                return null;
            }

            return this.buildQuestionAnswersList(x);
        }

        //
        //  #####   #   #    ###    #   #
        //  #       #   #   #   #   #  #    this obsolete shit 
        //  ###     #   #   #       ##          refactoring!
        //  #       #   #   #   #   #  #
        //  #        ###     ###    #   #
        //

        public bool Edit(int q_id, string text, int type)
        {
            this.Manager.Method("questions.edit");
            this.Manager.Params("qid", q_id);
            this.Manager.Params("text", text);
            this.Manager.Params("type", type);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        public bool Add(string text, int type)
        {
            this.Manager.Method("questions.add");
            this.Manager.Params("text", text);
            this.Manager.Params("type", type);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        public bool Delete(int q_id)
        {
            this.Manager.Method("questions.delete");
            this.Manager.Params("qid", q_id);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        public List<QuestionType> GetTypes()
        {
            this.Manager.Method("questions.getTypes");
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }
                return this.buildQuestionTypeList(x);
            }
            return null;
        }
        public List<QuestionAnswerVoter> GetAnswerVotes(int uid, int aid, QuestionSortMode? sort, int? needProfiles, int? count, int? offset)
        {
            this.Manager.Method("questions.getAnswerVotes");
            this.Manager.Params("aid", aid);
            this.Manager.Params("uid", uid);
            if (sort != null)
            {
                this.Manager.Params("sort", sort);
            }
            if (needProfiles != null)
            {
                this.Manager.Params("need_profiles", needProfiles);
            }
            if (count != null)
            {
                this.Manager.Params("count", count);
            }
            if (offset != null)
            {
                this.Manager.Params("offset", offset);
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                if (x.SelectSingleNode("/response").InnerText.Equals("0"))
                {
                    return null;
                }
                return this.buildQuestionVotersList(x);
            }
            return null;
        }

        public bool AddAnswer(int u_id, int q_id, string text)
        {
            this.Manager.Method("questions.addAnswer");
            this.Manager.Params("uid", u_id);
            this.Manager.Params("qid", q_id);
            this.Manager.Params("text", text);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        public bool DeleteAnswer(int u_id, int a_id)
        {
            this.Manager.Method("questions.deleteAnswer");
            this.Manager.Params("uid", u_id);
            this.Manager.Params("aid", a_id);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }

        public bool JoinAnswer(int u_id, int a_id)
        {
            this.Manager.Method("questions.joinAnswer");
            this.Manager.Params("uid", u_id);
            this.Manager.Params("aid", a_id);
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }


        public bool MarkAsViewed(object qid)
        {
            this.Manager.Method("questions.markAsViewed");
            if (qid != null)
            {
                if (qid is int)
                {
                    this.Manager.Params("aids", qid);
                }
                if (qid is string)
                {
                    this.Manager.Params("aids", qid);
                }
            }
            string resp = this.Manager.Execute().GetResponseString();
            if (this.Manager.MethodSuccessed)
            {
                XmlDocument x = this.Manager.GetXmlDocument(resp);
                return ((x.SelectSingleNode("/response").InnerText == "1") ? true : false);
            }
            return false;
        }
    }
}
