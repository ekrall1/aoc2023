package resourcestack

import (
	resources "github.com/pulumi/pulumi-azure-native-sdk/resources/v2"
	sdk "github.com/pulumi/pulumi-azure-native-sdk/v2"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi"
	"github.com/pulumi/pulumi/sdk/v3/go/pulumi/config"
)

type ResourceOutputs struct {
	ResourceGroup pulumi.StringOutput
}

func ResourceStack(ctx *pulumi.Context, providerLocation *sdk.Provider) (ResourceOutputs, error) {

	resourceGroupConfig := config.New(ctx, "resourceGroupConfig")

	resourceGroup, err := resources.NewResourceGroup(ctx, "aoc2023", &resources.ResourceGroupArgs{
		Location:          pulumi.String(config.Require(ctx, "providerLocation")),
		ResourceGroupName: pulumi.String(resourceGroupConfig.Require("name")),
	}, pulumi.Provider(providerLocation))

	if err != nil {
		return ResourceOutputs{}, err
	}

	return ResourceOutputs{
		ResourceGroup: resourceGroup.Name,
	}, nil

}
