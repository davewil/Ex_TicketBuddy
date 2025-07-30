using NUnit.Framework;

namespace Integration.Api;

public partial class UserControllerSpecs
{
    [Test]
    public void can_create_user()
    {
        Given(a_request_to_create_an_user);
        When(creating_the_user);
        And(requesting_the_user);
        Then(the_user_is_created);
    }
    
    [Test]
    public void can_update_user()
    {
        Given(an_user_exists);
        And(a_request_to_update_the_user);
        When(updating_the_user);
        And(requesting_the_updated_user);
        Then(the_user_is_updated);
    }
    
    [Test]
    public void can_list_users()
    {
        Given(an_user_exists);
        And(another_user_exists);
        When(listing_the_users);
        Then(the_users_are_listed);
    }
}