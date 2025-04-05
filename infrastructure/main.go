package main

import (
	sdk "github.com/pulumi/pulumi-azure-native-sdk/v2"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi/config"

	DevopsProjectStack "aoc2023/devopsprojectstack"
	ResourceStack "aoc2023/resourcestack"
)

func main() {

	pulumi.Run(func(ctx *pulumi.Context) error {
		provider, err := sdk.NewProvider(ctx, "provider-location", &sdk.ProviderArgs{
			Location:       pulumi.String(config.Require(ctx, "providerLocation")),
			SubscriptionId: pulumi.String(config.Require(ctx, "subscriptionId")),
		})

		if err != nil {
			return nil
		}

		resource, err := ResourceStack.ResourceStack(ctx, provider)

		if err != nil {
			return nil
		}

		devopsProject, err := DevopsProjectStack.DevopsStack(ctx, provider, resource.ResourceGroup)

		ctx.Export("resource group for aoc2023", resource.ResourceGroup)
		ctx.Export("devops project name", devopsProject.ProjectName)
		ctx.Export("sp client id", devopsProject.ServicePrincipalClientId)
		ctx.Export("sp clienttenant", devopsProject.ServicePrincipalTenant)
		ctx.Export("secret", devopsProject.ServicePrincipalSecret)
		ctx.Export("pipeline id", devopsProject.PipelineId)

		return nil
	})

}
