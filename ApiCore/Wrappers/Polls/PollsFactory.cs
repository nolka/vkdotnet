using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ApiCore.Polls
{
    public class PollsFactory: BaseFactory
    {
        public PollsFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private PollAnswerCollection buildAnswers(XmlNodeList data)
        {
            if (data != null && data.Count > 0)
            {
                PollAnswerCollection pac = new PollAnswerCollection();
                foreach (XmlNode n in data)
                {
                    XmlUtils.UseNode(n);
                    PollAnswer pa = new PollAnswer();
                    pa.Id = XmlUtils.Int("id");
                    pa.Rate = XmlUtils.Float("rate");
                    pa.Text = XmlUtils.String("text");
                    pa.Votes = XmlUtils.Int("votes");
                    pac.Add(pa);
                }
                return pac;
            }
            return null;
        }

        /// <summary>
        /// Get poll information by id
        /// </summary>
        /// <param name="ownerId">Poll owner id</param>
        /// <param name="pollId">Poll id</param>
        /// <returns>Poll information if all ok, null if not ok</returns>
        public PollEntry GetById(int? ownerId, int pollId)
        {
            this.Manager.Method("polls.getById", new object[] { "poll_id", pollId, "owner_id", ownerId });
            XmlNode result = this.Manager.Execute().GetResponseXml();
            XmlUtils.UseNode(result);
            PollEntry p = new PollEntry();
            p.Id = XmlUtils.Int("poll_id");
            p.OwnerId = XmlUtils.Int("owner_id");
            p.DateCreated = CommonUtils.FromUnixTime(XmlUtils.Int("created"));
            p.Question = XmlUtils.String("question");
            p.Votes = XmlUtils.Int("votes");
            p.AnswerId = XmlUtils.Int("answer_id");
            p.Answers = this.buildAnswers(result.SelectNodes("answers/answer"));

            return p;
        }

        public bool AddVote(int? ownerId, int pollId, int answeId)
        {
            this.Manager.Method("polls.addVote", new object[] { "poll_id", pollId, "answer_id", answeId, "owner_id", ownerId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

        public bool DeleteVote(int? ownerId, int pollId, int answeId)
        {
            this.Manager.Method("polls.deleteVote", new object[] { "poll_id", pollId, "answer_id", answeId, "owner_id", ownerId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.BoolVal();
        }

    }
}
