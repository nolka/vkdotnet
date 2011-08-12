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
            this.Manager.Method("polls.getById");
            this.Manager.Params("poll_id", pollId);
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
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

            return null;
        }

        public bool AddVote(int? ownerId, int pollId, int answeId)
        {
            this.Manager.Method("polls.addVote");
            this.Manager.Params("poll_id", pollId);
            this.Manager.Params("answer_id", answeId);
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }

            return false;
        }

        public bool DeleteVote(int? ownerId, int pollId, int answeId)
        {
            this.Manager.Method("polls.deleteVote");
            this.Manager.Params("poll_id", pollId);
            this.Manager.Params("answer_id", answeId);
            if (ownerId != null)
            {
                this.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = this.Manager.Execute().GetResponseXml();
            if (this.Manager.MethodSuccessed)
            {
                XmlUtils.UseNode(result);
                return XmlUtils.BoolVal();
            }

            return false;
        }

    }
}
