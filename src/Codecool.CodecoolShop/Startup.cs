using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Codecool.CodecoolShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Product/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
            });

            SetupInMemoryDatabases();
        }

        private void SetupInMemoryDatabases()
        {
            IProductDao productDataStore = ProductDaoMemory.GetInstance();
            IProductCategoryDao productCategoryDataStore = ProductCategoryDaoMemory.GetInstance();
            ISupplierDao supplierDataStore = SupplierDaoMemory.GetInstance();

            Supplier fromSoftware = new Supplier() { Name = "FromSoftware inc.", Description = "Games", Id = 1};
            supplierDataStore.Add(fromSoftware);
            Supplier hopoo = new Supplier() { Name = "Hopoo Games", Description = "Games", Id = 2};
            supplierDataStore.Add(hopoo);
            Supplier valve = new Supplier() { Name = "Valve", Description = "Games", Id = 3};
            supplierDataStore.Add(valve);
            ProductCategory RPG = new ProductCategory() { Name = "Adventure", Department = "Games", Description = "Games", Id = 1};
            productCategoryDataStore.Add(RPG);
            ProductCategory rogueLike = new ProductCategory() { Name = "Rogue-like", Department = "Games", Description = "Games", Id = 2};
            productCategoryDataStore.Add(rogueLike);
            ProductCategory shooter = new ProductCategory() { Name = "Shooter", Department = "Games", Description = "Games", Id = 3 };
            productCategoryDataStore.Add(shooter);
            ProductCategory puzzle = new ProductCategory() { Name = "Puzzle", Department = "Games", Description = "Games", Id = 4};
            productCategoryDataStore.Add(puzzle);

            productDataStore.Add(new Product() { Name = "Counter-Strike: Global Offensive" , DefaultPrice = 15m, Currency = "EURO", ProductCategory = shooter, Supplier = valve, imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/730/header.jpg?t=1641233427", Description = @"Counter-Strike: Global Offensive (CS: GO) expands upon the team-based action gameplay that it pioneered when it was launched 19 years ago.

CS: GO features new maps, characters, weapons, and game modes, and delivers updated versions of the classic CS content (de_dust2, etc.).

'Counter - Strike took the gaming industry by surprise when the unlikely MOD became the most played online PC action game in the world almost immediately after its release in August 1999,
                ' said Doug Lombardi at Valve. 'For the past 12 years,
                it has continued to be one of the most - played games in the world,
                headline competitive gaming tournaments and selling over 25 million units worldwide across the franchise.CS: GO promises to expand on CS' award-winning gameplay and deliver it to gamers on the PC as well as the next gen consoles and the Mac.' "});

            productDataStore.Add(new Product() {Name = "Left 4 Dead", DefaultPrice = 8.2m, ProductCategory = shooter, Supplier = valve, Currency = "EURO", imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/500/header.jpg?t=1623087651", Description = @"From Valve (the creators of Counter-Strike, Half-Life and more) comes Left 4 Dead, a co-op action horror game for the PC and Xbox 360 that casts up to four players in an epic struggle for survival against swarming zombie hordes and terrifying mutant monsters.
Set in the immediate aftermath of the zombie apocalypse, L4D's survival co-op mode lets you blast a path through the infected in four unique “movies,” guiding your survivors across the rooftops of an abandoned metropolis, through rural ghost towns and pitch-black forests in your quest to escape a devastated Ground Zero crawling with infected enemies. Each 'movie' is comprised of five large maps, and can be played by one to four human players, with an emphasis on team-based strategy and objectives.
New technology dubbed 'the AI Director' is used to generate a unique gameplay experience every time you play. The Director tailors the frequency and ferocity of the zombie attacks to your performance, putting you in the middle of a fast-paced, but not overwhelming, Hollywood horror movie." });

            productDataStore.Add(new Product() {Name = "Left 4 Dead 2", DefaultPrice = 8.2m, Currency = "EURO", Supplier = valve, ProductCategory = shooter, imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/550/header.jpg?t=1657220736", Description = @"Set in the zombie apocalypse, Left 4 Dead 2 (L4D2) is the highly anticipated sequel to the award-winning Left 4 Dead, the #1 co-op game of 2008.
This co-operative action horror FPS takes you and your friends through the cities, swamps and cemeteries of the Deep South, from Savannah to New Orleans across five expansive campaigns.
You'll play as one of four new survivors armed with a wide and devastating array of classic and upgraded weapons. In addition to firearms, you'll also get a chance to take out some aggression on infected with a variety of carnage-creating melee weapons, from chainsaws to axes and even the deadly frying pan.
You'll be putting these weapons to the test against (or playing as in Versus) three horrific and formidable new Special Infected. You'll also encounter five new uncommon common infected, including the terrifying Mudmen.
Helping to take L4D's frantic, action-packed gameplay to the next level is AI Director 2.0. This improved Director has the ability to procedurally change the weather you'll fight through and the pathways you'll take, in addition to tailoring the enemy population, effects, and sounds to match your performance. L4D2 promises a satisfying and uniquely challenging experience every time the game is played, custom-fitted to your style of play." });

            productDataStore.Add(new Product()
            {
                Name = "Portal", DefaultPrice = 8.2m, Currency = "EURO", ProductCategory = puzzle, Supplier = valve,
                imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/400/header.jpg?t=1608593358", Description =
                    @"Portal™ is a new single player game from Valve. Set in the mysterious Aperture Science Laboratories, Portal has been called one of the most innovative new games on the horizon and will offer gamers hours of unique gameplay.

The game is designed to change the way players approach, manipulate, and surmise the possibilities in a given environment; similar to how Half-Life® 2's Gravity Gun innovated new ways to leverage an object in any given situation.

Players must solve physical puzzles and challenges by opening portals to maneuvering objects, and themselves, through space."
            });

            productDataStore.Add(new Product() { Name = "Portal 2", DefaultPrice = 8.2m, Currency = "EURO", Supplier = valve, ProductCategory = puzzle, imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/620/header.jpg?t=1610490805", Description = @"Portal 2 draws from the award-winning formula of innovative gameplay, story, and music that earned the original Portal over 70 industry accolades and created a cult following.

The single-player portion of Portal 2 introduces a cast of dynamic new characters, a host of fresh puzzle elements, and a much larger set of devious test chambers. Players will explore never-before-seen areas of the Aperture Science Labs and be reunited with GLaDOS, the occasionally murderous computer companion who guided them through the original game.

The game’s two-player cooperative mode features its own entirely separate campaign with a unique story, test chambers, and two new player characters. This new mode forces players to reconsider everything they thought they knew about portals. Success will require them to not just act cooperatively, but to think cooperatively.
" });
            
            productDataStore.Add(new Product() { Name = "Risk of Rain 2", DefaultPrice = 24.9m, Currency = "EURO", Description = @"Over a dozen handcrafted locales await, each packed with challenging monsters and enormous bosses that oppose your continued existence. Fight your way to the final boss and escape or continue your run indefinitely to see just how long you can survive. A unique scaling system means both you and your foes limitlessly increase in power over the course of a game." , ProductCategory = rogueLike, Supplier = hopoo, imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/632360/header.jpg?t=1660063598" });
            
            productDataStore.Add(new Product() { Name = "ELDEN RING", DefaultPrice = 60.0m, Currency = "EURO", Description = @"Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.

                In the Lands Between ruled by Queen Marika the Eternal,
                the Elden Ring,
                the source of the Erdtree,
                has been shattered.

                Marika's offspring, demigods all, claimed the shards of the Elden Ring known as the Great Runes, and the mad taint of their newfound strength triggered a w ar: The Shattering. A war that meant abandonment by the Greater Will.

                And now the guidance of grace will be brought to the Tarnished who were spurned by the grace of gold and exiled from the Lands Between.Ye dead who yet live,
                your grace long lost,
                follow the path to the Lands Between beyond the foggy sea to stand before the Elden Ring.", ProductCategory = RPG, Supplier = fromSoftware, imgURL = "https://cdn.akamai.steamstatic.com/steam/apps/1245620/header.jpg?t=1654259241" });

            RPG.FeaturedProduct = productDataStore.GetBy("ELDEN RING");
            rogueLike.FeaturedProduct = productDataStore.GetBy("Risk of Rain 2");
            shooter.FeaturedProduct = productDataStore.GetBy("Counter-Strike: Global Offensive");
            puzzle.FeaturedProduct = productDataStore.GetBy("Portal 2");
        }
    }
}
