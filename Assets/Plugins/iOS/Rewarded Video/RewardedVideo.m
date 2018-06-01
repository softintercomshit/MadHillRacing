//
//  RewardedVideo.m
//  Unity-iPhone
//
//  Created by Ivan on 3/21/17.
//
//

#import "RewardedVideo.h"
@import GoogleMobileAds;

static NSString *const kAdUnitIDKey = @"ca-app-pub-2319820223323110/2653516181";

@interface RewardedVideo()<GADRewardBasedVideoAdDelegate>

@end

@implementation RewardedVideo

+(RewardedVideo *)show{
    static RewardedVideo *instance;
    
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        instance = [RewardedVideo new];
    });
    
    [instance showRewardedVideo];
    return instance;
}

-(void)showRewardedVideo{
    [GADRewardBasedVideoAd sharedInstance].delegate = self;
    [[GADRewardBasedVideoAd sharedInstance] loadRequest:[GADRequest request]
                                           withAdUnitID:kAdUnitIDKey];
}

- (void)rewardBasedVideoAdDidReceiveAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd{
    [rewardBasedVideoAd presentFromRootViewController:[UIApplication sharedApplication].keyWindow.rootViewController];
}

-(void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd didRewardUserWithReward:(GADAdReward *)reward{
    UnitySendMessage("AdsControl", "rewardedVideoWatched", "0");
}

- (void)rewardBasedVideoAd:(GADRewardBasedVideoAd *)rewardBasedVideoAd didFailToLoadWithError:(NSError *)error{
    NSLog(@"%@", error);
}

@end
