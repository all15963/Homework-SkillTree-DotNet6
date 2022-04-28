using System.Text;
using Microsoft.EntityFrameworkCore;
using MVCHomework6.Data.Database;

namespace MVCHomework6.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BlogDbContext(serviceProvider.GetRequiredService<DbContextOptions<BlogDbContext>>()))
            {
                if (context.Articles.Any())
                {
                    return;
                }
                var tmp = new List<Articles>();
                var tags = new StringBuilder();
                for (int i = 1; i <= 20; i++)
                {
                    var tag = RandomTag();
                    tmp.Add(new Articles
                    {
                        Id = Guid.NewGuid(),
                        Title = $"第{i}筆部落格",
                        Body = LoremIpsum(i),
                        CoverPhoto = $"https://fakeimg.pl/750x300/?retina=1&text=這是第{i}篇&font=noto",
                        CreateDate = DateTime.UtcNow.AddDays(i),
                        Tags = tag,
                    });
                    tags.Append(tag + ",");
                }

                var tagCloud = tags.ToString()
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .GroupBy(d => d)
                    .Select(d => new { Key = d.Key, Amount = d.Count() })
                    .ToList();
                foreach (var item in tagCloud)
                {
                    context.TagCloud.Add(new TagCloud
                    {
                        Id = Guid.NewGuid(),
                        Name = item.Key,
                        Amount = item.Amount
                    });
                }
                context.Articles.AddRange(tmp);
                context.SaveChanges();
            }
        }

        private static string RandomTag()
        {
            var tags = new List<string>() { "SkillTree", "twMVC", "demoshop", "Dotblogs", "MVC" };
            //先決定要取幾個
            var take = Enumerable.Range(1, 5).OrderBy(d => Guid.NewGuid()).First();
            //再亂數取幾個
            return string.Join(",", tags.OrderBy(d => Guid.NewGuid()).Take(take));

        }

        private static string LoremIpsum(int i)
        {
            return new List<string>
                             {
                                 $"<p>中央流行疫情指揮中心今(18)日表示，為感謝台灣耳鼻喉頭頸外科醫學會號召全臺會員，投入政府成立的社區篩檢站，貢獻專業，共同抗疫，目前已有320位醫師願意參與。今天特別邀請該學會理事長陳穆寬醫師及新北市醫師公會理事長周慶明醫師蒞臨記者會，鼓勵醫界一起響應，共同抗疫。指揮中心說明，醫界在防疫中扮演非常重要的角色，疑似病例通報、個案治療等第一線防疫工作，都讓醫事人員承受高風險及高壓力，今天許多醫師不畏辛勞，主動投入社區篩檢站，對國內防疫工作推動更是一大助益。陳穆寬理事長強調，在嚴峻疫情考驗下，感染源不明的本土案例出現，會逐漸影響整體基層醫療服務量能；政府成立社區篩檢站，篩檢可能個案，可減少疑似個案社區活動，並達到醫療分流、分工合作，降低病毒傳播風險；因此號召學會會員發揮耳鼻喉科專業，配合國家防疫指揮系統，全力支援社區篩檢站工作，協力控制疫情，目前已有320位醫師願意參與。新北市醫師公會理事長周慶明指出，耳鼻喉科醫師平時就專門照顧呼吸道疾病患者，這方面的專業能力非常出色，若積極投入，必定能提高篩檢準確度。針對新北市基層診所醫師所做的相關問卷調查也顯示，新北市醫師皆有接受良好訓練，除投入社區篩檢站，亦願意配合防疫，協助COVID - 19疫苗注射、集中檢疫所、居家隔離或居家檢疫者通訊診療及照護工作。指揮中心呼籲，現在是國內防疫的重要時刻，各界的力量都是防疫的後盾，請民眾配合做好各項防疫措施及個人防護工作，共同守住防疫陣線。</p>"
                                 , $"<p>天真過有居然個人都，就是了一家最近，我沒上個請問月日，兩真的沒太好了，什麼一是很有，可以多就不是。以有說好直想事，然想到麼害怕誕：錯誤本來個假覺得自邊的人的事⋯總覺得太誇張去不會，再好不會好⋯然到部分都想著來應該？下都今天的讓自己準備過該第一次⋯個沒次還魔法尊重生氣要這樣愛我：的人可一個人果就，這兩根本沒真什麼事案該是⋯在台擊為他，是什是威介意所以的時候謝謝喜。</p>"
                                 , $"<p>感覺，警察通常控，了的重出來的得有點怪人的，都可以的現在伊對方說，果是有我也好電話，延甚至的貓你怎麼為什有點。到有一但是喔喔起來國中，實際選擇的粉想睡覺哈哈哈一，真實⋯這麼快總之我也，喔喔你這個來想：想過剛好他們那麼多。發現原靠沒關係一直以的有，早上有點可不都的新文者情：十文字過如果也沒有，每次都，忍不杯出來：然後到現好好休想開場然他。抱著的東西很，睡好的以一也會跟著的關真的很。直痛約價格⋯真的會有時候祝福沒有了但，顆更好的感想突然有點像。</p>"
                                 , $"<p>想什麼動真的是，的做的下去的，沒抽到包裝們家小時候回增加⋯次的是發也有真的居然方的吃的，去哪紙看⋯了有看完的吧。標尼不喜歡，來如此棒的見去買也沒大家⋯林都可能會的那種音樂厲⋯聊因為我麼可愛廠商，面底為有多少說好為什小可愛，為什麼希然後不。</p>"
                                 , $"<p>樣我真的他一直的人，不得來做都不知熱，和了大好意是不的話的騷擾，也就如果不，出來就等做的留言，每次都有這麼的是。看的個新只是的謝謝幾年作者的，到推特一這真的化，我就沒的年，的方式結束會覺得招係明天嗚嗚宿。易再加上七預告我也，行為務不第一張：確定是只有兩，的下不一樣：不會出到時候這樣的人，兩隻印象中他們家不錯麼的。</p>"
                                 ,
                                 $"<p>想的不不愧是也是我也有，看了下好好跟其他真的只，總現罪代的遺言看。光推死真的好，其實是分的顏色上過就，原來是得好看，收我也嚇死當初，明有什還是過這種，主要死了花湯感受問題還是。他說享就是不是很天就是，了吧前有進去的角色，一說很等待我一個。沒錯才發真的快，我現在得一下好：沒有的一種可愛的根本不，就不會會先，我每特殊日本阿伯⋯還是可能會⋯</p>"
                                 ,
                                 $"<p>好真的為太我得很開我真的，死阿所以可怕哪裡麼說知道會。原來而言不夠結果他投幣。來只有時候，也沒的原也請還是現在也⋯你個不會位置果然還，完全沒我也打完靠北已經是：先生。天被處不來，下也在場次為什麼除本沒有，我明有一個前面對方己但真的我一，的拿出個越不多已經這個準備好⋯稿會跟就算了，聖誕卡覺得不。都可沒門次不畫你不是，滿肚子文這確實是味還真的。以前事的好像後再，想厲害然後被搞不好，就是在也不雖月我是說，才是不是我頭髮生民們在，是沒有追個想請觀察也不⋯同時也之類會被自己。</p>"
                             }
                  .OrderBy(d=>Guid.NewGuid())
                  .First();
        }
    }
}
