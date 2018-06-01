#import <SICAds.h>
#import "RewardedVideo.h"
#import "MKStoreKit.h"

static NSArray<NSString*> *const inAppBundles = @[@"com.appmoss.madhillracing.5000coins", @"com.appmoss.madhillracing.20000coins", @"com.appmoss.madhillracing.50000coins"];

NSString *deviceLanguage() {
    NSString *languageCode = [[NSLocale currentLocale] objectForKey:NSLocaleLanguageCode];
    return languageCode;
}

extern "C" {
    void showRewardedVideo() {
        [RewardedVideo show];
    }
    
    void showSICAds(){
        dispatch_async(dispatch_get_main_queue(), ^{
            [SICAds show];
        });
    }
    
    void buyCoins(int indexProduct) {
        [[MKStoreKit sharedKit] initiatePaymentRequestForProductWithIdentifier:inAppBundles[indexProduct]];
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        [defaults setInteger:indexProduct forKey:@"indexProduct"];
        [defaults synchronize];
    }
    
    char* getDeviceLanguage() {
        const char *languageCode = [deviceLanguage() cStringUsingEncoding:NSUTF8StringEncoding];
        
        if (languageCode == NULL)
            return NULL;
        
        char* res = (char*)malloc(strlen(languageCode) + 1);
        strcpy(res, languageCode);
        
        return res;
    }
}
