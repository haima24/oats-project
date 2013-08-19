using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public abstract class AbsResponseQuestion
    {
        public IEnumerable<UserInTestDetail> InTestDetails { get; set; }
        public IEnumerable<UserInTest> InTests { get; set; }
        public virtual IEnumerable<List<List<int>>> ListCoupleIds { get; set; }
        private bool isOneIdChecked = false;
        public virtual string NonChoiceText { get { return string.Empty; } }
        public int QuestionID { get; set; }
        public bool IsOneIdChecked
        {
            get { return isOneIdChecked; }
            set { isOneIdChecked = value; }
        }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string LabelOrder { get; set; }
        public string QuestionTitle { get; set; }
        public List<int> CheckedUserIds { get; set; }
        public decimal? NonChoiceScore { get; set; }
        public virtual decimal? UserNonChoiceScore { get { return 0; } }
        public decimal? ChoiceScore { get; set; }
        public decimal? UserScore
        {
            get
            {
                decimal? score = 0;
                if (ResponseAnswers != null) { score = ResponseAnswers.Sum(i => i.UserAnsScore); }
                //round
                score = score.RoundTwo();
                return score;
            }
        }
        public abstract IEnumerable<List<int>> Ids { get; }
        public abstract int TotalUsersInTest { get; }
        public abstract string CorrectPercent { get; }
        public abstract string CorrectPraction { get; }
        public abstract List<ResponseAnswer> ResponseAnswers { get; }

        protected AbsResponseQuestion(Question question, List<int> checkIds)
        {
            if (checkIds.Count == 1)
            {
                isOneIdChecked = true;
            }
            QuestionID = question.QuestionID;
            ImageUrl = question.ImageUrl;
            Type = question.QuestionType.Type;
            LabelOrder = question.LabelOrder;
            QuestionTitle = question.QuestionTitle;
            NonChoiceScore = question.RealNoneChoiceScore();
            var score = question.Answers.Sum(i => i.RealScore());
            var calculateScore = (!score.HasValue || score < 0) ? 0 : score;
            ChoiceScore = calculateScore.RoundTwo();
            CheckedUserIds = checkIds;
            InTests = question.Test.UserInTests.FilterInTestsOnAttempSetting();
            InTestDetails = InTests.SelectMany(i => i.UserInTestDetails).Where(i => i.QuestionID == question.QuestionID);
        }
    }
    public class ResponseQuestionRadio : AbsResponseQuestion
    {
        private int _totalUsersInTest = 0;
        private IEnumerable<List<int>> _ids = null;
        private List<ResponseAnswer> _responseAnswers = null;
        private string _correctPercent = String.Empty;
        private string _correctPraction = string.Empty;
        public ResponseQuestionRadio(Question question, List<int> checkids)
            : base(question, checkids)
        {
            _totalUsersInTest = InTests.Count();
            _ids = InTestDetails.Where(k => checkids.Contains(k.UserInTest.UserID)).Select(i =>
            {
                var listid = new List<int>();
                if (i.AnswerIDs != null)
                {
                    var idStrings = i.AnswerIDs.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    idStrings.ForEach(k =>
                    {
                        var tempid = 0;
                        if (int.TryParse(k, out tempid))
                        {
                            listid.Add(tempid);
                        }
                    });
                }
                return listid;
            });
            var rightAnsId = 0;
            var rightAns = question.Answers.FirstOrDefault(k => k.IsRight);
            if (rightAns != null) { rightAnsId = rightAns.AnswerID; }

            var userRightCount = Ids.Count(k => k.Contains(rightAnsId));
            var correctPer = ExtensionModel.SafeDivide(userRightCount, TotalUsersInTest);
            _correctPercent = correctPer.ToPercent();
            _correctPraction = TotalUsersInTest == 0 ? "0" : userRightCount + "/" + TotalUsersInTest;
            var answers = question.Answers.ToList();
            _responseAnswers = new List<ResponseAnswer>();
            answers.ForEach(k =>
            {
                var ans = new ResponseAnswer(this, k);
                _responseAnswers.Add(ans);
            });
        }

        public override IEnumerable<List<int>> Ids
        {
            get { return _ids; }
        }

        public override int TotalUsersInTest
        {
            get { return _totalUsersInTest; }
        }

        public override string CorrectPercent
        {
            get { return _correctPercent; }
        }

        public override string CorrectPraction
        {
            get { return _correctPraction; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return _responseAnswers; }
        }
    }
    public class ResponseQuestionMultile : AbsResponseQuestion
    {
        private int _totalUsersInTest = 0;
        private IEnumerable<List<int>> _ids = null;
        private List<ResponseAnswer> _responseAnswers = null;
        private string _correctPercent = String.Empty;
        private string _correctPraction = string.Empty;
        public ResponseQuestionMultile(Question question, List<int> checkids)
            : base(question, checkids)
        {
            _totalUsersInTest = InTests.Count();
            _ids = InTestDetails.Where(k => checkids.Contains(k.UserInTest.UserID)).Select(i =>
            {
                var listid = new List<int>();
                if (i.AnswerIDs != null)
                {
                    var idStrings = i.AnswerIDs.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    idStrings.ForEach(k =>
                    {
                        var tempid = 0;
                        if (int.TryParse(k, out tempid))
                        {
                            listid.Add(tempid);
                        }
                    });
                }
                return listid;
            });
            var rightAnsIds = question.Answers.Where(k => k.IsRight).Select(k => k.AnswerID).ToList();

            var userRightCount = Ids.Count(k => k.All(i => rightAnsIds.Contains(i)));
            var correctPer = ExtensionModel.SafeDivide(userRightCount, TotalUsersInTest);
            _correctPercent = correctPer.ToPercent();
            _correctPraction = TotalUsersInTest == 0 ? "0" : userRightCount + "/" + TotalUsersInTest;
            var answers = question.Answers.ToList();
            _responseAnswers = new List<ResponseAnswer>();
            answers.ForEach(k =>
            {
                var ans = new ResponseAnswer(this, k);
                _responseAnswers.Add(ans);
            });
        }

        public override IEnumerable<List<int>> Ids
        {
            get { return _ids; }
        }

        public override int TotalUsersInTest
        {
            get { return _totalUsersInTest; }
        }

        public override string CorrectPercent
        {
            get { return _correctPercent; }
        }

        public override string CorrectPraction
        {
            get { return _correctPraction; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return _responseAnswers; }
        }
    }
    public class ResponseQuestionMatching : AbsResponseQuestion
    {
        private int _totalUsersInTest = 0;
        private IEnumerable<List<List<int>>> _ids = null;
        private List<ResponseAnswer> _responseAnswers = null;
        private string _correctPercent = String.Empty;
        private string _correctPraction = string.Empty;
        public ResponseQuestionMatching(Question question, List<int> checkids)
            : base(question, checkids)
        {
            _totalUsersInTest = InTests.Count();
            _ids = InTestDetails.Where(t => checkids.Contains(t.UserInTest.UserID)).Select(i =>
            {
                var listid = new List<List<int>>();
                if (i.AnswerIDs != null)
                {
                    var idStrings = i.AnswerIDs.Split(';').ToList();
                    idStrings.ForEach(k =>
                    {
                        var twoIdString = k.Split(',').ToList();
                        var twoIds = new List<int>();
                        twoIdString.ForEach(j =>
                        {
                            var tempid = 0;
                            if (int.TryParse(j, out tempid))
                            {
                                twoIds.Add(tempid);
                            }
                        });
                        listid.Add(twoIds);
                    });
                }
                return listid;
            });

            var userRightCount = ListCoupleIds.Count(k => k.All(i =>
            {
                var result = false;
                var firstId = i.ElementAtOrDefault(0);
                var secondId = i.ElementAtOrDefault(1);
                if (firstId != 0)
                {
                    var answer = question.Answers.FirstOrDefault(o => o.AnswerID == firstId);
                    if (answer != null && secondId != 0)
                    {
                        var dependAns = answer.AnswerChilds.FirstOrDefault(q => q.AnswerID == secondId);
                        if (dependAns != null)
                        {
                            result = true;
                        }
                    }
                }
                return result;
            }));
            var correctPer = ExtensionModel.SafeDivide(userRightCount, TotalUsersInTest);
            _correctPercent = correctPer.ToPercent();
            _correctPraction = TotalUsersInTest == 0 ? "0" : userRightCount + "/" + TotalUsersInTest;
            var answers = question.Answers.Where(i => !i.DependencyAnswerID.HasValue).ToList();
            _responseAnswers = new List<ResponseAnswer>();
            answers.ForEach(k =>
            {
                var ans = new ResponseAnswer(this, k);
                _responseAnswers.Add(ans);
            });
        }

        public override IEnumerable<List<List<int>>> ListCoupleIds
        {
            get
            {
                return _ids;
            }
        }

        public override IEnumerable<List<int>> Ids
        {
            get { return null; }
        }

        public override int TotalUsersInTest
        {
            get { return _totalUsersInTest; }
        }

        public override string CorrectPercent
        {
            get { return _correctPercent; }
        }

        public override string CorrectPraction
        {
            get { return _correctPraction; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return _responseAnswers; }
        }
    }
    public class ResponseQuestionEssay : AbsResponseQuestion
    {
        private string _correctPraction = String.Empty;

        private string _nonChoiceText = string.Empty;
        private decimal? _userNonScore = 0;

        public ResponseQuestionEssay(Question question, List<int> checkIds)
            : base(question, checkIds)
        {
            var count = InTestDetails.Count();
            var point = InTestDetails.Sum(i => i.RealNonChoiceScore());
            decimal? averagePoint = decimal.Zero;
            if (count != 0) { averagePoint = point / count; }
            if (checkIds.Count == 0)
            {
                var id = checkIds.FirstOrDefault();
                if (id != 0)
                {
                    var detail = InTestDetails.FirstOrDefault(k => k.UserInTest.UserID == id);
                    if (detail != null)
                    {
                        averagePoint = detail.RealNonChoiceScore() ?? 0;
                    }
                }
            }
            var maxPoint = question.RealNoneChoiceScore().RoundTwo();
            _correctPraction = String.Format("{0:0.00} pt (of {1} pt)", averagePoint, maxPoint);
            if (checkIds.Count == 1)
            {
                var id = checkIds.FirstOrDefault();
                var detail = InTestDetails.FirstOrDefault(i => i.UserInTest.UserID == id);
                if (detail != null)
                {
                    _nonChoiceText = detail.AnswerContent;
                    _userNonScore = detail.RealNonChoiceScore();
                    _userNonScore = _userNonScore.RoundTwo();
                }
            }
        }

        public override decimal? UserNonChoiceScore
        {
            get
            {
                return _userNonScore ?? 0;
            }
        }

        public override string NonChoiceText
        {
            get
            {
                return _nonChoiceText;
            }
        }

        public override IEnumerable<List<int>> Ids
        {
            get { return null; }
        }

        public override int TotalUsersInTest
        {
            get { return 0; }
        }

        public override string CorrectPercent
        {
            get { return string.Empty; }
        }

        public override string CorrectPraction
        {
            get { return _correctPraction; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return null; }
        }
    }
    public class ResponseQuestionShort : AbsResponseQuestion
    {
        private string _nonChoiceText = string.Empty;
        private string _correctPraction = String.Empty;
        private decimal? _userNonScore = 0;
        public ResponseQuestionShort(Question question, List<int> checkIds)
            : base(question, checkIds)
        {
            var count = InTestDetails.Count();
            var point = InTestDetails.Sum(i => i.RealNonChoiceScore());
            decimal? averagePoint = decimal.Zero;
            if (count != 0) { averagePoint = point / count; }
            if (checkIds.Count == 0)
            {
                var id = checkIds.FirstOrDefault();
                if (id != 0)
                {
                    var detail = InTestDetails.FirstOrDefault(k => k.UserInTest.UserID == id);
                    if (detail != null)
                    {
                        averagePoint = detail.RealNonChoiceScore() ?? 0;
                    }
                }
            }
            var maxPoint = question.RealNoneChoiceScore().RoundTwo();
            _correctPraction = String.Format("{0:0.00} pt (of {1} pt)", averagePoint, maxPoint);

            if (checkIds.Count == 1)
            {
                var id = checkIds.FirstOrDefault();
                var detail = InTestDetails.FirstOrDefault(i => i.UserInTest.UserID == id);
                if (detail != null)
                {
                    _nonChoiceText = detail.AnswerContent;
                    _userNonScore = detail.RealNonChoiceScore();
                    _userNonScore = _userNonScore.RoundTwo();
                }
            }
        }

        public override decimal? UserNonChoiceScore
        {
            get
            {
                return _userNonScore ?? 0;
            }
        }

        public override string NonChoiceText
        {
            get
            {
                return _nonChoiceText;
            }
        }

        public override IEnumerable<List<int>> Ids
        {
            get { return null; }
        }

        public override int TotalUsersInTest
        {
            get { return 0; }
        }

        public override string CorrectPercent
        {
            get { return string.Empty; }
        }

        public override string CorrectPraction
        {
            get { return _correctPraction; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return null; }
        }
    }
    public class ResponseQuestionText : AbsResponseQuestion
    {


        public ResponseQuestionText(Question question, List<int> checkIds)
            : base(question, checkIds)
        {

        }

        public override IEnumerable<List<int>> Ids
        {
            get { return null; }
        }

        public override int TotalUsersInTest
        {
            get { return 0; }
        }

        public override string CorrectPercent
        {
            get { return string.Empty; }
        }

        public override string CorrectPraction
        {
            get { return string.Empty; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return null; }
        }
    }
    public class ResponseQuestionImage : AbsResponseQuestion
    {
        public ResponseQuestionImage(Question question, List<int> checkIds)
            : base(question, checkIds)
        {
        }

        public override IEnumerable<List<int>> Ids
        {
            get { return null; }
        }

        public override int TotalUsersInTest
        {
            get { return 0; }
        }

        public override string CorrectPercent
        {
            get { return string.Empty; }
        }

        public override string CorrectPraction
        {
            get { return string.Empty; }
        }

        public override List<ResponseAnswer> ResponseAnswers
        {
            get { return null; }
        }
    }
    public class ResponseAnswer
    {
        public string AnswerContent { get; set; }
        public string AnswerMatchingContent { get; set; }
        public bool IsRight { get; set; }
        public decimal? Score { get; set; }
        public string AnswerPercent { get; set; }
        private int countOfUserChooseThisAns = 0;
        private decimal? userAnsScore = 0;

        public decimal? UserAnsScore
        {
            get { return userAnsScore; }
            set { userAnsScore = value; }
        }
        private bool userChooseThisAnswer = false;

        public bool UserChooseThisAnswer
        {
            get { return userChooseThisAnswer; }
            set { userChooseThisAnswer = value; }
        }
        public ResponseAnswer(AbsResponseQuestion question, Answer answer)
        {
            if (question.Ids != null)
            {
                countOfUserChooseThisAns = (question.Ids.Count(k => k.Contains(answer.AnswerID)));
            }
            else if (question.ListCoupleIds != null)
            {
                //in this case, we count the user matching right couples
                countOfUserChooseThisAns = question.ListCoupleIds.Count(k =>
                {
                    return k.Any(q =>
                    {
                        var result = false;
                        var firstId = q.ElementAtOrDefault(0);
                        var secondId = q.ElementAtOrDefault(1);
                        if (firstId == answer.AnswerID)
                        {
                            var dependAns = answer.AnswerChilds.FirstOrDefault(w => w.AnswerID == secondId);
                            if (dependAns != null)
                            {
                                result = true;
                            }
                        }
                        return result;
                    });
                });
                var dependAnswer = answer.AnswerChilds.FirstOrDefault();
                if (dependAnswer != null)
                {
                    AnswerMatchingContent = dependAnswer.AnswerContent;
                }
            }
            AnswerPercent = ExtensionModel.SafeDivide(countOfUserChooseThisAns,question.TotalUsersInTest).ToPercent();
            AnswerContent = answer.AnswerContent;
            IsRight = answer.IsRight;
            var ansScore=answer.RealScore();
            Score = ansScore.RoundTwo();
            if (question.IsOneIdChecked)
            {
                if (question.Ids != null)
                {
                    var ids = question.Ids.FirstOrDefault();
                    if (ids != null)
                    {
                        userChooseThisAnswer = ids.Contains(answer.AnswerID);
                        if (userChooseThisAnswer) {
                            var score = answer.RealScore();
                            userAnsScore = (!score.HasValue || score < 0) ? 0 : score; 
                        }
                    }
                }
                else if (question.ListCoupleIds != null)
                {
                    var coupleIds = question.ListCoupleIds.FirstOrDefault();
                    if (coupleIds != null)
                    {
                        var dependId = 0;
                        var dependAns = answer.AnswerChilds.FirstOrDefault();
                        if (dependAns != null)
                        {
                            dependId = dependAns.AnswerID;
                        }
                        var ids = new List<int>(){
                            answer.AnswerID,
                            dependId
                        };
                        userChooseThisAnswer = coupleIds.Contains(ids, (f, s) =>
                        {
                            return f.ElementAtOrDefault(0) == s.ElementAtOrDefault(0) && f.ElementAtOrDefault(1) == s.ElementAtOrDefault(1);
                        });
                        if (userChooseThisAnswer) {
                            var score = answer.RealScore();
                            userAnsScore = (!score.HasValue || score < 0) ? 0 : score;
                        }
                    }
                }
            }
        }
    }

    public class ResponseTest
    {
        public bool IsHaveData { get; set; }
        private string testTakenDate = String.Empty;
        public string TestTakenDate
        {
            get { return testTakenDate; }
        }
        private OATSDBEntities db = null;
        public List<int> CheckedUserIds { get; set; }
        public decimal? TotalScoreOfTest { get; set; }
        public List<AbsResponseQuestion> Questions { get; set; }

        public int ResponseUserListCount { get { return ResponseUserList.Count; } }
        public int CheckedUserIdsCount { get { return CheckedUserIds.Count; } }
        public List<ResponseUserItem> ResponseUserList { get; set; }
        public string TestTitle { get; set; }
        public ResponseTest(Test test)
        {
            var details = test.UserInTests.FilterInTestsOnAttempSetting().Select(i => i.UserID).ToList();
            InitResponseTest(test, details);
        }
        public ResponseTest(Test test, List<int> checkIds)
        {
            InitResponseTest(test, checkIds);
        }
        private void InitResponseTest(Test test, List<int> checkIds)
        {
            db = SingletonDb.Instance();
            var inTestsValid = test.UserInTests.FilterInTestsOnAttempSetting();
            IsHaveData = inTestsValid.Count > 0;
            TestTitle = test.TestTitle;
            CheckedUserIds = checkIds;
            if (checkIds.Count == 1)
            {
                var id = checkIds.FirstOrDefault();
                var inTest = inTestsValid.FirstOrDefault(i => i.UserID == id);
                if (inTest != null)
                {
                    testTakenDate = String.Format("{0:dd MMM yyyy HH:mm tt}", inTest.TestTakenDate);
                }

            }
            TotalScoreOfTest = test.Questions.TotalRealScore();
            var details = inTestsValid.ToList();
            ResponseUserList = new List<ResponseUserItem>();
            details.ForEach(i =>
            {
                if (i.User != null)
                {
                    var item = new ResponseUserItem();
                    item.UserLabel = !string.IsNullOrEmpty(i.User.Name) ? i.User.Name : i.User.UserMail;
                    item.UserPercent = TotalScoreOfTest != 0 && TotalScoreOfTest.HasValue?((decimal)i.Score / TotalScoreOfTest).ToPercent():"0%";
                    item.UserID = i.User.UserID;
                    ResponseUserList.Add(item);
                }
            });
            var questions = test.Questions.ToList();
            questions = questions.OrderBy(i => i.SerialOrder).ToList();
            Questions = new List<AbsResponseQuestion>();
            if (checkIds.Count > 0)
            {
                questions.ForEach(i =>
                {
                    AbsResponseQuestion item = null;
                    switch (i.QuestionType.Type)
                    {
                        case "Radio":
                            item = new ResponseQuestionRadio(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        case "Multiple":
                            item = new ResponseQuestionMultile(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        case "Essay":
                            item = new ResponseQuestionEssay(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        case "ShortAnswer":
                            item = new ResponseQuestionShort(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        case "Text":
                            item = new ResponseQuestionText(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        case "Image":
                            item = new ResponseQuestionImage(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        case "Matching":
                            item = new ResponseQuestionMatching(i, CheckedUserIds);
                            Questions.Add(item);
                            break;
                        default:
                            break;
                    }
                });
            }
        }
    }
    public class ResponseUserItem : IUserItem
    {
        public string UserLabel { get; set; }
        public string UserPercent { get; set; }
        public int UserID { get; set; }
    }

}