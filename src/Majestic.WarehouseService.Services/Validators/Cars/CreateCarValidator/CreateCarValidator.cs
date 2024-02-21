﻿using Majestic.WarehouseService.Models.Misc;
using Majestic.WarehouseService.Models.v1.CreateCars.Request;

namespace Majestic.WarehouseService.Services.Validators.Cars.CreateCarValidator
{
    public class CreateCarValidator : ICreateCarValidator
    {
        public ServiceResult Validate(CreateCarRequest request)
        {
            var errors = new Dictionary<string, IList<string>>();

            #region ValidateCarName
            var carNameErrors = ValidateCarName(request.CarName);
            if (carNameErrors != null)
            {
                errors.Add("CarName", carNameErrors);
            }
            #endregion

            #region ValidateModelName
            var modelNameErrors = ValidateModelName(request.ModelName);
            if (modelNameErrors != null)
            {
                errors.Add("ModelName", modelNameErrors);
            }
            #endregion

            #region ValidateOwnerName
            var ownerNameErrors = ValidateOwnerName(request.OwnerName);
            if (ownerNameErrors != null)
            {
                errors.Add("OwnerName", ownerNameErrors);
            }
            #endregion

            #region ValidateDealersPrice
            var dealersPriceErrors = ValidateDealersPrice(request.DealersPrice, request.OwnersPrice);
            if (dealersPriceErrors != null)
            {
                errors.Add("DealersPrice", dealersPriceErrors);
            }
            #endregion

            #region ValidateOwnersPrice
            var ownersPriceErrors = ValidateOwnersPrice(request.OwnersPrice);
            if (ownersPriceErrors != null)
            {
                errors.Add("OwnersPrice", ownersPriceErrors);
            }
            #endregion

            #region ValidateDealerNote
            var dealerNotesErrors = ValidateDealerNote(request.DealerNotes);
            if (dealerNotesErrors != null)
            {
                errors.Add("DealerNotes", dealerNotesErrors);
            }
            #endregion

            if (errors.Any())
            {
                return new ServiceResult
                {
                    Message = "Some fields was invalid",
                    Data = errors
                };
            }

            return new ServiceResult(true);
        }

        #region ValidateCarName
        private List<string> ValidateCarName(string carName)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(carName))
            {
                errors.Add("The required field");
                return errors;
            }

            const int MaxCarNameLength = 100;
            if (carName.Length > MaxCarNameLength)
            {
                errors.Add($"Car name max length is {MaxCarNameLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateModelName
        private List<string> ValidateModelName(string modelName)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(modelName))
            {
                errors.Add("The required field");
                return errors;
            }

            const int MaxModelNameLength = 100;
            if (modelName.Length > MaxModelNameLength)
            {
                errors.Add($"Model name max length is {MaxModelNameLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateOwnerName
        private List<string> ValidateOwnerName(string ownerName)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ownerName))
            {
                errors.Add("The required field");
                return errors;
            }

            const int MaxOwnerNameLength = 100;
            if (ownerName.Length > MaxOwnerNameLength)
            {
                errors.Add($"Owner name max length is {MaxOwnerNameLength}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateDealersPrice
        private List<string> ValidateDealersPrice(decimal dealerPrice, decimal ownerPrice)
        {
            var errors = new List<string>();

            if (dealerPrice <= 0)
            {
                errors.Add("Dealer's price cannot be negative");
                return errors;
            }

            const int MaxPrice = 1_000_000;
            if (dealerPrice > MaxPrice)
            {
                errors.Add($"Dealer's rice cannot be more than {MaxPrice}");
                return errors;
            }

            if (dealerPrice > ownerPrice)
            {
                errors.Add($"Dealer's price cannot be more that owner's price");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateOwnersPrice
        private List<string> ValidateOwnersPrice(decimal ownerPrice)
        {
            var errors = new List<string>();

            if (ownerPrice <= 0)
            {
                errors.Add("Owner's price cannot be negative");
                return errors;
            }

            const int MaxPrice = 1_000_000;
            if (ownerPrice > MaxPrice)
            {
                errors.Add($"Owner's price cannot be more than {MaxPrice}");
                return errors;
            }

            return null;
        }
        #endregion

        #region ValidateDealerNote
        private List<string> ValidateDealerNote(string dealerNotes)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dealerNotes))
            {
                errors.Add("Dealer's notes cannot be empty");
                return errors;
            }

            const int MaxDealersNotesLength = 1000;
            if (dealerNotes.Length > MaxDealersNotesLength)
            {
                errors.Add($"Dealer's notes cannot be more than 1000 {MaxDealersNotesLength}");
                return errors;
            }

            return null;
        }
        #endregion
    }
}
